using Algorithms.SphereCovering;
using SOM.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace SOM
{
	public class TwoDimensionalSOM<TPoint2D> : BaseSOM<TPoint2D> where TPoint2D : IVector<double>
	{
		public TwoDimensionalSOM(SOMSettings settings, IList<TPoint2D> dataVectors, Func<IEnumerable<double>, TPoint2D> vectorGenerator, Topology topology)
			: base(settings, dataVectors, vectorGenerator, topology)
		{
		}

		protected override void InitNetwork(Topology topology)
		{
			switch (topology)
			{
				case Topology.Sphere:
				default:
					InitSphereNetwork(this.dataVectors.Count);
					break;
			}
		}

		protected override void InitNetwork(bool[,] topology)
		{
			throw new NotImplementedException();
		}

		protected override void InitSphereNetwork(int networkSize)
		{
			//networkSize *= 10;

			base.InitSphereNetwork(networkSize);

			var xValues = this.dataVectors.Select(v => v["x"]);
			var yValues = this.dataVectors.Select(v => v["y"]);

			var xWidth = (xValues.Max() - xValues.Min());
			var yWidth = (yValues.Max() - yValues.Min());

			var centerX = xValues.Average();
			var centerY = yValues.Average();
			//var centerX = 0;
			//var centerY = 0;

			//var rand = new Random();

			//var centerX = rand.NextDouble() * (xValues.Max() - xValues.Min()) + xValues.Min();
			//var centerY = rand.NextDouble() * (yValues.Max() - yValues.Min()) + yValues.Min();

			var radius = (xWidth < yWidth)
				? xWidth / 100D * settings.NetworkRadiusPercent
				: yWidth / 100D * settings.NetworkRadiusPercent;

			this.networkVectors = new List<TPoint2D>(networkSize);
			//this.finalNetworkVectors = new List<TPoint2D>(networkSize);
			this.networkTopologyMatrix = new double?[networkSize, networkSize];
			this.networkReadiness = new Dictionary<TPoint2D, bool>(networkSize);

			var circlePoints = SphereTools.GetCirclePoints(networkSize, radius, (centerX, centerY));

			for (int i = 0; i < networkSize; i++)
			{
				//creating new cell
				this.networkVectors.Add(generateVector(new double[] { circlePoints[i].X, circlePoints[i].Y }));
				//this.networkTopologyMatrix[i, i] = 0;
				this.networkReadiness.Add(this.networkVectors[i], false);


				//if (i > 0)

				for (int j = 0; j < i; j++)
				{
					if (i < networkSize / 2 + j)
					{
						//var distance = GetDistance(this.networkVectors[i], this.networkVectors[j]);
						this.networkTopologyMatrix[i, j] = i - j;
						this.networkTopologyMatrix[j, i] = i - j;
					}
					else
					{
						this.networkTopologyMatrix[i, j] = networkSize - (i - j);
						this.networkTopologyMatrix[j, i] = networkSize - (i - j);
					}
				}

				////for last point, fill distance to first point
				//if (i == networkSize - 1)
				//{
				//	var distance = GetDistance(this.networkVectors[i], this.networkVectors[0]);
				//	this.networkTopologyMatrix[i, 0] = distance;
				//	this.networkTopologyMatrix[0, i] = distance;
				//}
			}

			if (settings.UseDistancePenalties)
			{
				this.networkDistancePenalties = new Dictionary<TPoint2D, double>();

				foreach (var vector in this.networkVectors)
					this.networkDistancePenalties.Add(vector, networkDistancePenaltiesDefaultValue);
			}


			n = networkSize / 2;

			//Console.WriteLine(string.Join("\r\n", this.networkVectors.Select(x => $"{x[0]};{x[1]}")));

		}

		protected override double GetDistance(TPoint2D first, TPoint2D second)
		{
			return Math.Sqrt(Math.Pow(first["x"] - second["x"], 2) + Math.Pow(first["y"] - second["y"], 2));
		}

		protected List<TPoint2D> data;
		Func<TPoint2D, TPoint2D, double> getDistanceFunc;

		public override void ProcessEpoch()
		{
			//var unprocessedDataVectors = this.dataVectors.ToDictionary(x => x, x => false);

			//var networkVectors = this.networkVectors.Except(this.networkReadiness.Where(x => x.Value).Select(x => x.Key)).ToList();
			//data = this.dataVectors.ToList();

			if (settings.UseDistancePenalties)
				getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint) * this.networkDistancePenalties[networkPoint];
			else
				getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint);


			foreach (var dataVector in dataVectors.Where(x => this.dataReadiness[x] == false)) //except matched ones																										   //while (unprocessedDataVectors.Any(x => x.Value == false))
			{
				//алгоритм идет пока не this.networkReadiness.All(x => x.Value == true);
				//var dataVector = unprocessedDataVectors.First(x => x.Value == false).Key;

				var orderedNetworkVectors = networkVectors
											.Except(this.networkReadiness.Where(x => x.Value).Select(x => x.Key))
											.OrderBy(x => getDistanceFunc(x, dataVector))
											.Select(x => (x, getDistanceFunc(x, dataVector))).ToList();

				var (closestNetworkVector, distance) = orderedNetworkVectors.FirstOrDefault();

				if (closestNetworkVector != null)
				{
					if (distance > settings.RoundPrecision)
					{
						for (var i = 0; i < closestNetworkVector.Count; i++)
							closestNetworkVector[i] = closestNetworkVector[i] + settings.LearningCoefficient * (dataVector[i] - closestNetworkVector[i]);
					}
					else
					{
						for (var i = 0; i < closestNetworkVector.Count; i++)
							closestNetworkVector[i] = dataVector[i];

						this.networkReadiness[closestNetworkVector] = true;
						this.dataReadiness[dataVector] = true;

						//this.networkVectors.Remove(closestNetworkVector);
						//this.dataVectors.Remove(dataVector);

						//this.finalNetworkVectors.Add(closestNetworkVector);
					}

					if (settings.UseElasticity)
					{
						foreach (var networkVector in networkVectors.Where(x => this.networkReadiness[x] == false))
						{
							if (networkVector.Equals(closestNetworkVector))
								continue;

							var elasticityCoef = GetElasticityCoefficient(networkVector, closestNetworkVector);

							if (elasticityCoef > 0)
								for (var i = 0; i < closestNetworkVector.Count; i++)
									networkVector[i] = networkVector[i] +
										settings.LearningCoefficient *
										(dataVector[i] - networkVector[i]) *
										elasticityCoef;
						}
					}

					if (settings.UseDistancePenalties)
						this.networkDistancePenalties[closestNetworkVector] *= 1D - settings.PenaltiesIncreasingCoefficient / 100D; //1% -> newValue = oldValue * (100% - 1%)

					if (settings.LearningFadingCoefficient.HasValue)
						settings.LearningCoefficient *= 1D - settings.LearningFadingCoefficient.Value / 100D;

					n *= 1D - settings.CooperationDistance / 100D;
				}
			}
		}

		public override IEnumerable<int> ProcessEpochIteration()
		{
			if (data == null || data.Count == 0)
			{
				data = dataVectors.Where(x => this.dataReadiness[x] == false).ToList();
			}

			if (settings.UseDistancePenalties)
				getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint) * this.networkDistancePenalties[networkPoint];
			else
				getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint);

			//foreach (var dataVector in data.Where(x => this.dataReadiness[x] == false)) //except matched ones																										   //while (unprocessedDataVectors.Any(x => x.Value == false))
			//{
			var dataVector = data.FirstOrDefault();

			//алгоритм идет пока не this.networkReadiness.All(x => x.Value == true);

			var orderedNetworkVectors = networkVectors
										.Except(this.networkReadiness.Where(x => x.Value).Select(x => x.Key))
										.OrderBy(x => getDistanceFunc(x, dataVector))
										.Select(x => (x, getDistanceFunc(x, dataVector))).ToList();

			var (closestNetworkVector, distance) = orderedNetworkVectors.FirstOrDefault();

			if (closestNetworkVector != null)
			{
				if (distance > settings.RoundPrecision)
				{
					for (var i = 0; i < closestNetworkVector.Count; i++)
						closestNetworkVector[i] = closestNetworkVector[i] + settings.LearningCoefficient * (dataVector[i] - closestNetworkVector[i]);
				}
				else
				{
					for (var i = 0; i < closestNetworkVector.Count; i++)
						closestNetworkVector[i] = dataVector[i];

					this.networkReadiness[closestNetworkVector] = true;
					this.dataReadiness[dataVector] = true;

					//this.networkVectors.Remove(closestNetworkVector);
					//this.dataVectors.Remove(dataVector);

					//this.finalNetworkVectors.Add(closestNetworkVector);
				}

				if (settings.UseElasticity)
				{
					foreach (var networkVector in networkVectors.Where(x => this.networkReadiness[x] == false))
					{
						if (networkVector.Equals(closestNetworkVector))
							continue;

						var elasticityCoef = GetElasticityCoefficient(networkVector, closestNetworkVector);

						if (elasticityCoef > 0)
							for (var i = 0; i < closestNetworkVector.Count; i++)
								networkVector[i] = networkVector[i] +
									settings.LearningCoefficient *
									(dataVector[i] - networkVector[i]) *
									elasticityCoef;
					}
				}

				if (settings.UseDistancePenalties)
					this.networkDistancePenalties[closestNetworkVector] *= 1D - settings.PenaltiesIncreasingCoefficient / 100D; //1% -> newValue = oldValue * (100% - 1%)

				if (settings.LearningFadingCoefficient.HasValue)
					settings.LearningCoefficient *= 1D - settings.LearningFadingCoefficient.Value / 100D;

				n *= 1D - settings.CooperationDistance / 100D;
			}

			data.RemoveAt(0);
			yield return 0;
		}

		protected override double GetElasticityCoefficient(TPoint2D chosenVector, TPoint2D otherVector)
		{
			var distance = GetDistance(chosenVector, otherVector);
			var pointsDistance = this.networkTopologyMatrix[this.networkVectors.IndexOf(chosenVector), this.networkVectors.IndexOf(otherVector)].Value;
			//var coef = Math.Exp(-1 * Math.Pow(distance, 2) / (n * n) * pointsDistance.Value);
			var coef = Math.Exp(-1 * Math.Pow(pointsDistance, 2) / n);
			//var coef = 1 / distance;

			return coef >= settings.CooperationThreshold ? coef : 0;
		}

		public override IEnumerable<TPoint2D> BuildMap()
		{

			var stopwatch = Stopwatch.StartNew();

			while (!FinishCondition)
				ProcessEpoch();

			stopwatch.Stop();

			Console.WriteLine(stopwatch.Elapsed);

			return this.networkVectors.ToList();
		}
	}
}

using Algorithms.SphereCovering;
using Algorithms.Utility.StructuresLinking;
using SOM.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SOM
{
	public class TwoDimensionalSOM<TPoint2D> : BaseSOM<TPoint2D> where TPoint2D : IVector<double>
	{
		public TwoDimensionalSOM(SOMSettings settings, IList<TPoint2D> dataVectors, Topology topology)
			: base(settings, dataVectors, topology)
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
			networkSize = (int)Math.Round(networkSize * settings.NetworkSizeMultiplier);

			base.InitSphereNetwork(networkSize);

			var xValues = this.dataVectors.Select(v => v["x"]);
			var yValues = this.dataVectors.Select(v => v["y"]);

			var xWidth = xValues.Max() - xValues.Min();
			var yWidth = yValues.Max() - yValues.Min();

			var centerX = xValues.Average();
			var centerY = yValues.Average();

			var radius = (xWidth < yWidth)
				? xWidth / 100D * settings.NetworkRadiusPercent
				: yWidth / 100D * settings.NetworkRadiusPercent;

			this.networkVectors = new List<TPoint2D>(networkSize);
			this.networkTopologyDistances = new Dictionary<TPoint2D, Dictionary<TPoint2D, double>>(networkSize);
			this.networkReadiness = new Dictionary<TPoint2D, bool>(networkSize);

			var circlePoints = SphereTools.GetCirclePoints(networkSize, radius, (centerX, centerY));

			for (int i = 0; i < networkSize; i++)
			{
				//creating new cell
				this.networkVectors.Add(Activator.CreateInstance<TPoint2D>());
				this.networkVectors.Last()["x"] = circlePoints[i].X;
				this.networkVectors.Last()["y"] = circlePoints[i].Y;

				this.networkReadiness.Add(this.networkVectors[i], false);

				this.networkTopologyDistances.Add(this.networkVectors[i], new Dictionary<TPoint2D, double>());

				for (int j = 0; j < i; j++)
				{
					if (i < networkSize / 2 + j)
					{
						this.networkTopologyDistances[this.networkVectors[i]].Add(this.networkVectors[j], i - j);
						this.networkTopologyDistances[this.networkVectors[j]].Add(this.networkVectors[i], i - j);
					}
					else
					{
						this.networkTopologyDistances[this.networkVectors[i]].Add(this.networkVectors[j], networkSize - (i - j));
						this.networkTopologyDistances[this.networkVectors[j]].Add(this.networkVectors[i], networkSize - (i - j));
					}
				}
			}

			if (settings.UseDistancePenalties)
			{
				this.networkDistancePenalties = new Dictionary<TPoint2D, double>();

				foreach (var vector in this.networkVectors)
					this.networkDistancePenalties.Add(vector, networkDistancePenaltiesDefaultValue);
			}

			this.workingNetworkVectors = this.networkVectors.ToList();
		}

		public override double GetDistance(TPoint2D first, TPoint2D second)
		{
			return Math.Sqrt(Math.Pow(first["x"] - second["x"], 2) + Math.Pow(first["y"] - second["y"], 2));
		}

		Func<TPoint2D, TPoint2D, double> getDistanceFunc;
		IList<TPoint2D> workingNetworkVectors;

		public override void ProcessIteration()
		{
			if (settings.UseDistancePenalties)
				getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint) * this.networkDistancePenalties[networkPoint];
			else
				getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint);

			foreach (var dataVector in dataVectors.Where(x => this.dataReadiness[x] == false)) //except matched ones
			{
				var orderedNetworkVectors = workingNetworkVectors
											.Select(x => (x, getDistanceFunc(x, dataVector)))
											.OrderBy(x => x.Item2);

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

						this.workingNetworkVectors.Remove(closestNetworkVector);
					}

					if (settings.UseElasticity && settings.CooperationCoefficient > settings.CooperationThreshold)
					{
						var networkVectorsToAdjust = workingNetworkVectors.ToList();
						networkVectorsToAdjust.Remove(closestNetworkVector);

						var elasticityCoefs = networkVectorsToAdjust.Select(x => GetNeighbourFunction(x, closestNetworkVector)).ToArray();

						for (int i = 0; i < networkVectorsToAdjust.Count; i++)
						{
							if (elasticityCoefs[i] > 0D)
								for (var j = 0; j < closestNetworkVector.Count; j++)
									networkVectorsToAdjust[i][j] = networkVectorsToAdjust[i][j] +
										settings.LearningCoefficient *
										(dataVector[j] - networkVectorsToAdjust[i][j]) *
										elasticityCoefs[i];
						}
						
						settings.CooperationCoefficient *= 1D - settings.CooperationFading / 100D;
					}

					if (settings.UseDistancePenalties)
						this.networkDistancePenalties[closestNetworkVector] *= 1D + settings.PenaltiesIncreasingCoefficient / 100D; //1% -> newValue = oldValue * (100% - 1%)

					if (settings.LearningFadingCoefficient.HasValue)
						settings.LearningCoefficient *= 1D - settings.LearningFadingCoefficient.Value / 100D;
				}
			}
		}

		protected List<TPoint2D> data;

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

			var dataVector = data.FirstOrDefault();

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
				}

				if (settings.UseElasticity)
				{
					foreach (var networkVector in networkVectors.Where(x => this.networkReadiness[x] == false))
					{
						if (networkVector.Equals(closestNetworkVector))
							continue;

						var elasticityCoef = GetNeighbourFunction(networkVector, closestNetworkVector);

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

				settings.CooperationCoefficient *= 1D - settings.CooperationFading / 100D;
			}

			data.RemoveAt(0);
			yield return 0;
		}

		protected override double GetNeighbourFunction(TPoint2D chosenVector, TPoint2D otherVector)
		{
			var distance = GetDistance(chosenVector, otherVector);
			var pointsDistance = this.networkTopologyDistances[chosenVector][otherVector];
			var coef = Math.Exp(-1 * Math.Pow(pointsDistance, 2) * distance / settings.CooperationCoefficient);
			return coef;
		}

		public override IEnumerable<TPoint2D> BuildMap()
		{
			var stopwatch = Stopwatch.StartNew();

			while (!FinishCondition)
			{
				Console.WriteLine($"{ProcessedVectors} - {stopwatch.Elapsed} | coef: {settings.LearningCoefficient} | length: {GetFullLength()} | n: {settings.CooperationCoefficient}");
				ProcessIteration();
			}

			stopwatch.Stop();

			Console.WriteLine(stopwatch.Elapsed);

			return this.networkVectors.Where(x => this.networkReadiness[x]).ToList();
		}
	}
}

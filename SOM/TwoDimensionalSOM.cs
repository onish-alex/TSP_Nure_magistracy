using Algorithms.SphereCovering;
using SOM.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SOM
{
    public class TwoDimensionalSOM<TPoint2D> : SOMBuilderBase<TPoint2D> where TPoint2D : IVector<double>
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
            var xValues = this.dataVectors.Select(v => v["x"]);
            var yValues = this.dataVectors.Select(v => v["y"]);

            var xWidth = (xValues.Max() - xValues.Min());
            var yWidth = (yValues.Max() - yValues.Min());

            var centerX = xValues.Average();
            var centerY = yValues.Average();
            //var centerX = 0;
            //var centerY = 0;

            var radius = (xWidth < yWidth)
                ? xWidth / 100D * radiusPercent
                : yWidth / 100D * radiusPercent;

            this.networkVectors = new List<TPoint2D>(networkSize);
            this.networkTopologyMatrix = new double?[networkSize, networkSize];
            this.networkReadiness = new Dictionary<TPoint2D, bool>(networkSize);

            var circlePoints = SphereTools.GetCirclePoints(networkSize, radius, (centerX, centerY));

            for (int i = 0; i < networkSize; i++)
            {
                //creating new cell
                this.networkVectors.Add(generateVector(new double[] { circlePoints[i].X, circlePoints[i].Y }));
                this.networkTopologyMatrix[i, i] = 0;
                this.networkReadiness.Add(this.networkVectors[i], false);

                //starting from second point, filling distance to previously added point
                if (i > 0)
                {
                    var distance = GetDistance(this.networkVectors[i], this.networkVectors[i - 1]);
                    this.networkTopologyMatrix[i, i - 1] = distance;
                    this.networkTopologyMatrix[i - 1, i] = distance;
                }

                //for last point, fill distance to first point
                if (i == networkSize - 1)
                {
                    var distance = GetDistance(this.networkVectors[i], this.networkVectors[0]);
                    this.networkTopologyMatrix[i, 0] = distance;
                    this.networkTopologyMatrix[0, i] = distance;
                }
            }

            if (settings.UseDistancePenalties)
            {
                this.networkDistancePenalties = new Dictionary<TPoint2D, double>();

                foreach (var vector in this.networkVectors)
                    this.networkDistancePenalties.Add(vector, networkDistancePenaltiesDefaultValue);
            }
        }

        protected override double GetDistance(TPoint2D first, TPoint2D second)
        {
            return Math.Sqrt(Math.Pow(first["x"] - second["x"], 2) + Math.Pow(first["y"] - second["y"], 2));
        }

        protected override void ProcessEpoch()
        {
            Func<TPoint2D, TPoint2D, double> getDistanceFunc;

            if (settings.UseDistancePenalties)
                getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint) * this.networkDistancePenalties[networkPoint];
            else
                getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint);

            foreach (var dataVector in this.dataVectors.Except(this.dataReadiness.Where(x => x.Value).Select(x => x.Key))) //except matched ones
            {
                //алгоритм идет пока не this.networkReadiness.All(x => x.Value == true);

                var (closestNetworkVector, distance) = 
                    this.networkVectors.Except(this.networkReadiness.Where(x => x.Value).Select(x => x.Key))
                                       .OrderBy(x => getDistanceFunc(x, dataVector))
                                       .Select(x => (x, getDistanceFunc(x, dataVector)))
                                       .FirstOrDefault();

                if (closestNetworkVector != null)
                {
                    if (settings.UseElasticity)
                    {
                        foreach (var networkVector in this.networkVectors.Except(new TPoint2D[] { closestNetworkVector }))
                        {
                            var elasticityCoef = GetElasticityCoefficient(networkVector, closestNetworkVector);


                            //TODO
                        }
                    }
                    else
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
                    }

                    if (settings.UseDistancePenalties)
                        this.networkDistancePenalties[closestNetworkVector] *= 1D - settings.PenaltiesIncreasingCoefficient / 100D; //1% -> newValue = oldValue * (100% - 1%)

                    if (settings.LearningFadingCoefficient.HasValue)
                        settings.LearningCoefficient *= 1D - settings.LearningFadingCoefficient.Value / 100D;
                }
            }
        }

        protected override double GetElasticityCoefficient(TPoint2D chosenVector, TPoint2D otherVector)
        {
            return 0D;
            //var distance = GetDistance(chosenVector, otherVector);

            //return Math.Pow(Math.Exp, 
        }

        public override IEnumerable<TPoint2D> BuildMap()
        {
            var stopwatch = Stopwatch.StartNew();

            while (this.networkReadiness.Any(x => x.Value == false))
                ProcessEpoch();

            stopwatch.Stop();

            Console.WriteLine(stopwatch.Elapsed);

            return this.networkVectors.ToList();
        }
    }
}

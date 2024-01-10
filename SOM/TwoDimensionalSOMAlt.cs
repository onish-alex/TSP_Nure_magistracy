using Algorithms.SphereCovering;
using SOM.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SOM
{
    public class TwoDimensionalSOMAlt<TPoint2D> : TwoDimensionalSOM<TPoint2D> where TPoint2D : IVector<double>
    {
        public TwoDimensionalSOMAlt(SOMSettings settings, IList<TPoint2D> dataVectors, Func<IEnumerable<double>, TPoint2D> vectorGenerator, Topology topology)
            : base(settings, dataVectors, vectorGenerator, topology)
        {
        }

        protected override void ProcessEpoch()
        {
            Func<TPoint2D, TPoint2D, double> getDistanceFunc;

            if (settings.UseDistancePenalties)
                getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint) * this.networkDistancePenalties[networkPoint];
            else
                getDistanceFunc = (networkPoint, dataPoint) => GetDistance(networkPoint, dataPoint);

            foreach (var networkVector in this.networkVectors.Except(this.networkReadiness.Where(x => x.Value).Select(x => x.Key))) //except matched ones
            {
                //алгоритм идет пока не this.networkReadiness.All(x => x.Value == true);

                var (closestDataVector, distance) = 
                    this.dataVectors.Except(this.dataReadiness.Where(x => x.Value).Select(x => x.Key))
                                       .OrderBy(x => getDistanceFunc(networkVector, x))
                                       .Select(x => (x, getDistanceFunc(networkVector, x)))
                                       .FirstOrDefault();

                if (closestDataVector != null)
                {
                    if (settings.UseElasticity)
                    {
                        foreach (var data in this.dataVectors.Except(new TPoint2D[] { closestDataVector }))
                        {
                            //var elasticityCoef = GetElasticityCoefficient(networkVector, closestNetworkVector);


                            //TODO
                        }
                    }
                    else
                    {
                        if (distance > settings.RoundPrecision)
                        {
                            for (var i = 0; i < networkVector.Count; i++)
                                networkVector[i] = networkVector[i] + settings.LearningCoefficient * (closestDataVector[i] - networkVector[i]);
                        }
                        else
                        {
                            for (var i = 0; i < networkVector.Count; i++)
                                networkVector[i] = closestDataVector[i];

                            this.networkReadiness[networkVector] = true;
                            this.dataReadiness[closestDataVector] = true;
                        }
                    }

                    if (settings.UseDistancePenalties)
                        this.networkDistancePenalties[networkVector] *= 1D - settings.PenaltiesIncreasingCoefficient / 100D; //1% -> newValue = oldValue * (100% - 1%)

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
    }
}

using Algorithms.SphereCovering;
using SOM.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SOM
{
    public class TwoDimensionalSOM<TPoint2D> : SOMBuilderBase<TPoint2D> where TPoint2D : IVector<double>
    {
        public TwoDimensionalSOM(IList<TPoint2D> dataVectors, Func<IEnumerable<double>, TPoint2D> vectorGenerator, Topology topology) 
            : base(dataVectors, vectorGenerator, topology)
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

            var centerX = xWidth / 2;
            var centerY = yWidth / 2;

            var radius = (xWidth < yWidth)
                ? xWidth / 100D * radiusPercent
                : yWidth / 100D * radiusPercent;

            this.networkVectors = new List<TPoint2D>(networkSize);
            this.networkTopologyMatrix = new double?[networkSize, networkSize];

            var circlePoints = SphereTools.GetCirclePoints(networkSize, radius, (centerX, centerY));

            for (int i = 0; i < networkSize; i++)
            {
                //creating new cell
                this.networkVectors.Add(generateVector(new double[] { circlePoints[i].X, circlePoints[i].Y }));

                this.networkTopologyMatrix[i, i] = 0;
                
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
        }

        protected override double GetDistance(TPoint2D first, TPoint2D second)
        {
            return Math.Sqrt(Math.Pow(first["x"] - second["x"], 2) + Math.Pow(first["y"] - second["y"], 2));
        }
    }
}

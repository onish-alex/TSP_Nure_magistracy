using Algorithms.Utility.StructuresLinking;
using Newtonsoft.Json;
using System;

namespace TSP.Core
{
	public class TSPNode : IVector<double>
	{
		[JsonProperty("N")] public string Name { get; set; }
		public double X { get; set; }
		public double Y { get; set; }

		public override string ToString() => string.Format("{0,5} {1,5} | {2,5}", X, Y, Name);

        public double this[string axis]
        {
            get
            {
                switch (axis)
                {
                    case "X":
                    case "x":
                        return X;

                    case "Y":
                    case "y":
                        return Y;

                    default:
                        return 0D;
                }
            }

            set
            {
                switch (axis)
                {
                    case "X":
                    case "x":
                        X = value;
                        break;

                    case "Y":
                    case "y":
                        Y = value;
                        break;
                }
            }
        }

        public double this[int index] 
        {
            get
            {
                return index switch
                {
                    0 => X,
                    1 => Y,
                    _ => throw new ArgumentException("TSPNode is two-dimensional")
                };
            }

            set 
            {
                switch (index)
                {
                    case 0: X = value; break;
                    case 1: Y = value; break;
                    default:  throw new ArgumentException("TSPNode is two-dimensional");
                };
            } 
        }

        public int Count => 2;
    }
}

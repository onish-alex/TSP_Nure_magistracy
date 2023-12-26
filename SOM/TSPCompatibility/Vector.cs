using System.Collections;
using System.Collections.Generic;

namespace SOM.TSPCompatibility
{
    public class Vector : IVector<double>
    {
        private IList<double> parameters;

        public Vector(IList<double> parameters)
        {
            this.parameters = parameters;
        }

        public double this[string axis]
        {
            get
            {
                switch(axis)
                {
                    case "X":
                    case "x":
                        return parameters[0];

                    case "Y":
                    case "y":
                        return parameters[1];

                    case "Z":
                    case "z":
                        return parameters[2];

                    default:
                        return 0D;
                }
            }
        }

        public double this[int index] { get => parameters[index]; set => parameters[index] = value; }


        public int Count => parameters.Count;

        public IEnumerator<double> GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return parameters.GetEnumerator();
        }

        public override string ToString()
        {
            return string.Join("; ", parameters);
        }
    }
}

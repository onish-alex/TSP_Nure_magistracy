using Newtonsoft.Json;

namespace TSP.Core
{
    public class TSPNode
    {
        [JsonProperty("N")] public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString() => string.Format("{0,5} {1,5} | {2,5}", X, Y, Name);
    }
}

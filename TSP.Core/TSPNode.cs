namespace TSP.Core
{
    public class TSPNode 
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString() => $"{Name} {X} {Y}";
    }
}

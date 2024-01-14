namespace TSP.Core
{
	public static class TSPModelLoader
	{
		//private const string NODES_START_LINE = "NODE_COORD_SECTION";
		//private const string SOLUTION_START_LINE = "TOUR_SECTION";
		//private const string SOLUTION_END_LINE = "-1";

		//public static TSPModel GetModelFromFile(string filePath)
		//{
		//    string[] lines = null;

		//    try
		//    {
		//        lines = File.ReadAllLines(filePath);
		//    }
		//    catch
		//    {
		//        throw new FileNotFoundException();
		//    }

		//    var nodesInStr = lines.SkipWhile(x => !x.Contains(NODES_START_LINE))
		//                          .Skip(1)
		//                          .SkipLast(1);

		//    var isExponentialNotation = false;

		//    if (nodesInStr.Any())
		//        if (nodesInStr.First().Contains("e+"))
		//            isExponentialNotation = true;

		//    var nodes = new List<TSPNode>();

		//    foreach (var node in nodesInStr)
		//    {
		//        var nodeParts = node.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		//        double x, y;

		//        if (isExponentialNotation)
		//        {
		//            var xParts = nodeParts[1].Split("e+", StringSplitOptions.RemoveEmptyEntries);
		//            x = double.Parse(xParts[0]) * Math.Pow(10, double.Parse(xParts[1]));

		//            var yParts = nodeParts[2].Split("e+", StringSplitOptions.RemoveEmptyEntries);
		//            y = double.Parse(yParts[0]) * Math.Pow(10, double.Parse(yParts[1]));
		//        }
		//        else
		//        {
		//            x = double.Parse(nodeParts[1]);
		//            y = double.Parse(nodeParts[2]);
		//        }

		//        var tspNode = new TSPNode()
		//        {
		//            Name = nodeParts[0],
		//            X = x,
		//            Y = y,
		//        };

		//        nodes.Add(tspNode);
		//    }

		//    return new TSPModel(nodes);
		//}

		//public static IList<TSPNode> GetSolutionFromFile(TSPModel model, string solutionFilePath)
		//{
		//    string[] lines = null;

		//    try
		//    {
		//        lines = File.ReadAllLines(solutionFilePath);
		//    }
		//    catch
		//    {
		//        throw new FileNotFoundException();
		//    }

		//    var nodeNamesInStr = lines.SkipWhile(x => !x.Contains(SOLUTION_START_LINE))
		//                          .Skip(1);

		//    var nodes = new List<TSPNode>();

		//    foreach (var nodeName in nodeNamesInStr)
		//    {
		//        if (nodeName == SOLUTION_END_LINE)
		//            break;

		//        var strParts = nodeName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

		//        foreach (var name in strParts)
		//            nodes.Add(model.Nodes.Single(x => x.Name == name));
		//    }

		//    return nodes;
		//}
	}
}

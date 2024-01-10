﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using TSP.Core;

namespace TSP.Examples
{
    public static class PreparedModelLoader
    {
        public static int? FractionLength = 1;

        private const string MODEL_EXTENSION = ".tsp";
        private const string OPTIMAL_TOUR_EXTENSION = ".opt.tour";

        private const string modelsFolder = "Models/";
        private const string solutionsFolder = "Solutions/";

        private const string NODES_START_LINE = "NODE_COORD_SECTION";
        private const string SOLUTION_START_LINE = "TOUR_SECTION";
        private const string SOLUTION_END_LINE = "-1";

        public static TSPModel GetModel(PreparedModelsEnum modelName)
        {
            string[] lines = null;
            string modelNameStr = Enum.GetName(typeof(PreparedModelsEnum), modelName);
            string fileName = modelNameStr + MODEL_EXTENSION;
            string filePath = Path.Combine(modelsFolder, fileName);

            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch
            {
                throw new FileNotFoundException();
            }

            var nodesInStr = lines.SkipWhile(x => !x.Contains(NODES_START_LINE))
                                  .Skip(1)
                                  .SkipLast(1);

            var isExponentialNotation = false;

            if (nodesInStr.Any())
                if (nodesInStr.First().Contains("e+"))
                    isExponentialNotation = true;

            var nodes = new List<TSPNode>();

            foreach (var node in nodesInStr)
            {
                var nodeParts = node.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                double x, y;

                if (isExponentialNotation)
                {
                    var xParts = nodeParts[1].Split("e+", StringSplitOptions.RemoveEmptyEntries);
                    x = double.Parse(xParts[0]) * Math.Pow(10, double.Parse(xParts[1]));

                    var yParts = nodeParts[2].Split("e+", StringSplitOptions.RemoveEmptyEntries);
                    y = double.Parse(yParts[0]) * Math.Pow(10, double.Parse(yParts[1]));
                }
                else
                {
                    var xSplitted = nodeParts[1].Split('.');
                    var xInt = xSplitted[0];
                    var xFractional = string.Empty;

                    if (xSplitted.Length > 1)
                        xFractional = (FractionLength.HasValue && FractionLength >= 0)
                            ? xSplitted[1][..FractionLength.Value]
                            : xSplitted[1];

                    var ySplitted = nodeParts[2].Split('.');
                    var yInt = ySplitted[0];
                    var yFractional = string.Empty;

                    if (xSplitted.Length > 1)
                        yFractional = (FractionLength.HasValue && FractionLength >= 0)
                            ? ySplitted[1][..FractionLength.Value]
                            : ySplitted[1];

                    x = double.Parse($"{xInt}{(string.IsNullOrEmpty(xFractional) ? string.Empty : $".{xFractional}")}");
                    y = double.Parse($"{yInt}{(string.IsNullOrEmpty(yFractional) ? string.Empty : $".{yFractional}")}");
                }

                var tspNode = new TSPNode()
                {
                    Name = nodeParts[0],
                    X = x,
                    Y = y,
                };

                nodes.Add(tspNode);
            }

            return new TSPModel(nodes);
        }

        public static IList<TSPNode> GetSolution(TSPModel model, PreparedModelsEnum modelName)
        {
            string[] lines = null;
            string modelNameStr = Enum.GetName(typeof(PreparedModelsEnum), modelName);
            string fileName = modelNameStr + OPTIMAL_TOUR_EXTENSION;
            string filePath = Path.Combine(solutionsFolder, fileName);

            try
            {
                lines = File.ReadAllLines(filePath);
            }
            catch
            {
                throw new FileNotFoundException();
            }

            var nodeNamesInStr = lines.SkipWhile(x => !x.Contains(SOLUTION_START_LINE))
                                  .Skip(1);

            var nodes = new List<TSPNode>();

            foreach (var nodeName in nodeNamesInStr)
            {
                if (nodeName == SOLUTION_END_LINE)
                    break;

                var strParts = nodeName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (var name in strParts)
                    nodes.Add(model.Nodes.Single(x => x.Name == name));
            }

            return nodes;
        }
    }
}

using System.Collections.Generic;

namespace TSP.Core
{
    public static class TSPExamples
    {
        private const string examplesFolder = "Examples/";
        private const string solutionsFolder = "Solutions/";

        public const string rd100 = examplesFolder + "rd100.tsp";
        public const string st70 = examplesFolder + "st70.tsp";
        public const string att48 = examplesFolder + "att48.tsp";
        public const string berlin52 = examplesFolder + "berlin52.tsp";
        public const string ch130 = examplesFolder + "ch130.tsp";
        public const string ch150 = examplesFolder + "ch150.tsp";
        public const string eil51 = examplesFolder + "eil51.tsp";
        public const string eil76 = examplesFolder + "eil76.tsp";
        public const string eil101 = examplesFolder + "eil101.tsp";
        public const string lin105 = examplesFolder + "lin105.tsp";
        public const string gr96 = examplesFolder + "gr96.tsp";

        public static readonly IDictionary<string, string> Solutions = new Dictionary<string, string>()
        {
            { rd100, solutionsFolder + "rd100.opt.tour" },
            { st70, solutionsFolder + "st70.opt.tour" },
            { att48, solutionsFolder + "att48.opt.tour" },
            { berlin52, solutionsFolder + "berlin52.opt.tour" },
            { ch130, solutionsFolder + "ch130.opt.tour" },
            { ch150, solutionsFolder + "ch150.opt.tour" },
            { eil51, solutionsFolder + "eil51.opt.tour" },
            { eil76, solutionsFolder + "eil76.opt.tour" },
            { eil101, solutionsFolder + "eil101.opt.tour" },
            { lin105, solutionsFolder + "lin105.opt.tour" },
            { gr96, solutionsFolder + "gr96.opt.tour" },
        };
    }
}

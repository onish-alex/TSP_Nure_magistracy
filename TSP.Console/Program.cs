﻿using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TSP.Core;

namespace TSP.ConsoleApp
{
    class Program
    {
        static void Main()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("en-US");
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

            var model = TSPModelLoader.GetModelFromFile(TSPExamples.gr96);

            var loadedRoute = TSPModelLoader.GetSolutionFromFile(model, TSPExamples.Solutions[TSPExamples.gr96]);

            var nodes = model.Nodes;

            var route = new List<TSPNode>()
            {
                nodes[6],
                nodes[32],
                nodes[43],
                nodes[34],
                nodes[33],
                nodes[38],
                nodes[56],
                nodes[84],
                nodes[72],
                nodes[77],
                nodes[92],
                nodes[67],
                nodes[60],
                nodes[59],
                nodes[23],
                nodes[24],
                nodes[53],
                nodes[57],
                nodes[61],
                nodes[63],
                nodes[66],
                nodes[65],
                nodes[64],
                nodes[27],
                nodes[26],
                nodes[42],
                nodes[45],
                nodes[10],
                nodes[12],
                nodes[17],
                nodes[18],
                nodes[19],
                nodes[16],
                nodes[13],
                nodes[11],
                nodes[9],
                nodes[5],
                nodes[46],
                nodes[47],
                nodes[52],
                nodes[37],
                nodes[35],
                nodes[30],
                nodes[31],
                nodes[36],
                nodes[29],
                nodes[28],
                nodes[0],
                nodes[2],
                nodes[1],
                nodes[4],
                nodes[3],
                nodes[14],
                nodes[20],
                nodes[22],
                nodes[25],
                nodes[21],
                nodes[49],
                nodes[39],
                nodes[40],
                nodes[41],
                nodes[44],
                nodes[48],
                nodes[51],
                nodes[50],
                nodes[70],
                nodes[80],
                nodes[76],
                nodes[71],
                nodes[58],
                nodes[68],
                nodes[55],
                nodes[54],
                nodes[69],
                nodes[78],
                nodes[79],
                nodes[81],
                nodes[74],
                nodes[85],
                nodes[88],
                nodes[86],
                nodes[87],
                nodes[73],
                nodes[82],
                nodes[90],
                nodes[89],
                nodes[83],
                nodes[75],
                nodes[91],
                nodes[94],
                nodes[95],
                nodes[93],
                nodes[62],
                nodes[15],
                nodes[8],
                nodes[7],
            };

            var route2 = new List<TSPNode>()
            {
                nodes[28],
                nodes[1],
                nodes[2],
                nodes[3],
                nodes[4],
                nodes[5],
                nodes[6],
                nodes[7],
                nodes[8],
                nodes[9],
                nodes[11],
                nodes[12],
                nodes[13],
                nodes[14],
                nodes[15],
                nodes[16],
                nodes[19],
                nodes[17],
                nodes[18],
                nodes[20],
                nodes[24],
                nodes[23],
                nodes[22],
                nodes[21],
                nodes[25],
                nodes[27],
                nodes[26],
                nodes[64],
                nodes[95],
                nodes[93],
                nodes[94],
                nodes[92],
                nodes[91],
                nodes[76],
                nodes[75],
                nodes[67],
                nodes[66],
                nodes[65],
                nodes[63],
                nodes[62],
                nodes[61],
                nodes[60],
                nodes[59],
                nodes[58],
                nodes[70],
                nodes[71],
                nodes[72],
                nodes[74],
                nodes[73],
                nodes[83],
                nodes[85],
                nodes[84],
                nodes[77],
                nodes[87],
                nodes[86],
                nodes[88],
                nodes[89],
                nodes[90],
                nodes[82],
                nodes[81],
                nodes[80],
                nodes[79],
                nodes[78],
                nodes[69],
                nodes[68],
                nodes[56],
                nodes[55],
                nodes[57],
                nodes[53],
                nodes[47],
                nodes[46],
                nodes[45],
                nodes[49],
                nodes[51],
                nodes[52],
                nodes[54],
                nodes[50],
                nodes[48],
                nodes[42],
                nodes[41],
                nodes[40],
                nodes[39],
                nodes[38],
                nodes[43],
                nodes[44],
                nodes[10],
                nodes[32],
                nodes[33],
                nodes[34],
                nodes[37],
                nodes[36],
                nodes[35],
                nodes[31],
                nodes[30],
                nodes[29],
                nodes[0],
            };

            var dist = model.GetDistance(route2);
        }
    }
}

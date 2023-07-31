using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSP.Desktop.Models.Entities
{
	public class Algorithm
	{
        public string Name { get; set; }
    }

    public enum AlgorithmState
    {
        AlgorithmCreated,
        AlgorithmLoaded,
        AlgorithmSaved
    }
}

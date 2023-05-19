using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony.Core.Utilities
{
    internal class Ant<TNode>
    {
        public IList<TNode> TravelledPathMemory;
        
        public Ant() 
        {
            TravelledPathMemory = new List<TNode>();        
        }
    }
}

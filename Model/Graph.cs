using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liu.Model
{
    public class Graph
    {
        public SortedDictionary<int, Work> Works { get; set; }
        public SortedDictionary<int, Work> StartPoints { get; set; }
        public SortedDictionary<int, Work> EndPoints { get; set; }
    }
}

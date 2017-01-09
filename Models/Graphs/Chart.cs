using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using Newtonsoft.Json;

namespace MPE.SS.Models.Graphs
{
    public class Chart
    {
        public Chart()
        {
            DataSets = new List<DataSet>();
        }

        public DataCollector DataCollector { get; set; }
        public string Server { get; set; }
        public string Name { get; set; }
        public List<DataSet> DataSets { get; set; }
    }
}

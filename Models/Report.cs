using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models.Graphs;

namespace MPE.SS.Models
{
    public class Report : Entity
    {
        public DateTime LastUpdated { get; set; }
        public string Alias { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public ReportItem ReportItem { get; set; }
        public Configuration Configuration { get; set; }
        public Graph Graph { get; set; }
        public List<Chart> Charts { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models
{
    public class Baseline : Entity
    {
        public string ChartAlias { get; set; }
        public string Server { get; set; }
        public Dictionary<DateTime, long> Collections { get; set; }
        public long Average { get; set; }

        public override string GenerateId()
        {
            return string.Format("{0}-{1}-{2}", GetType().Name, ChartAlias, Server);
        }
    }
}

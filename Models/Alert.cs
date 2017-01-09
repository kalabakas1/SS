using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;

namespace MPE.SS.Models
{
    public class Alert
    {
        public string Alias { get; set; }
        public string Dataset { get; set; }
        public List<AlertThreshold> Thresholds { get; set; }
    }
}

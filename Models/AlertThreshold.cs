using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;

namespace MPE.SS.Models
{
    public class AlertThreshold
    {
        public double Threshold { get; set; }
        public AlertLevel Level { get; set; }
        public int DurationInMin { get; set; }
    }
}

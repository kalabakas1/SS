using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;

namespace MPE.SS.Models
{
    public class Notification
    {
        public AlertLevel AlertLevel { get; set; }
        public string Message { get; set; }
    }
}

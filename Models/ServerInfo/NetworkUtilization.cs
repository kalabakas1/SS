using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models.ServerInfo
{
    public class NetworkUtilization
    {
        public double SentKiloBytesPerSec { get; set; }
        public double ReceivedKiloBytesPerSec { get; set; }
    }
}

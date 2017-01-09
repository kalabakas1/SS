using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models.HaProxy
{
    public class HapInfo : Data<Server>
    {
        public List<string> ProxyNames { get; set; }
        public List<HapServer> Servers { get; set; }
    }
}

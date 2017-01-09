using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models
{
    public class Connection
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return string.Format("{0} - {1}:{2}", Name, Host, Port);
        }
    }
}

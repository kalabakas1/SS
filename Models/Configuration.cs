using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models
{
    public class Configuration
    {
        public Configuration() { }

        public string Alias { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public List<Credential> Credentials { get; set; }
        public List<Server> Servers { get; set; }
        public List<Request> Requests { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models
{
    public class Request
    {
        public string Label { get; set; }
        public string Uri { get; set; }
        public bool Ssl { get; set; }
        public int StatusCode { get; set; }
        public bool Gzip { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models
{
    public class Website
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string State { get; set; }
        public string PhysicalPath { get; set; }
        public string[] Bindings { get; set; }
    }
}

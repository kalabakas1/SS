using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MPE.SS.Models
{
    public class Data<T> : Entity
    {
        public string Reference { get; set; }
    }
}

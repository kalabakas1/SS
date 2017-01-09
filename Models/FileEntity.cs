using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Models
{
    public abstract class FileEntity
    {
        public string FileName { get; set; }
        public string Extension { get; set; }
    }
}

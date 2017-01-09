using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;

namespace MPE.SS.Exceptions
{
    internal class InvalidValidationException : Exception
    {
        public ReportItem Report { get; set; }
    }
}

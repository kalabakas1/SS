using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Logic.Attributes
{
    public class CsvPosition : Attribute
    {
        public string FieldName { get; set; }
        public int Position { get; set; }
        public CsvPosition(
            string fieldName,
            int position)
        {
            FieldName = fieldName;
            Position = position;
        }
    }
}

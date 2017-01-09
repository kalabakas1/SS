using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Logic.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    internal class RedisFieldAttribute : Attribute
    {
        public string RedisFieldName { get; set; }

        public RedisFieldAttribute(string field)
        {
            RedisFieldName = field;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;

namespace MPE.SS.Models
{
    public class Entity : ICloneable
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Deleted { get; set; }
        public virtual string GenerateId()
        {
            return string.Format("{0}-{1}", GetType().Name, Guid.NewGuid());
        }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}

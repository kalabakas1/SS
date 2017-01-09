using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Interfaces
{
    internal interface IRepository<T>
    {
        T Save(T obj);

        void Remove(T obj);

        IEnumerable<T> GetAll();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Interfaces
{
    internal interface IValidator<T>
    {
        bool IsValid(T obj);
    }

    internal interface IValidator<T, TK> : IValidator<T>
    {
        bool IsValid(T obj, out TK message);
    }
}

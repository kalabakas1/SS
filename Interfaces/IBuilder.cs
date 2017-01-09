using System;

namespace MPE.SS.Interfaces
{
    internal interface IBuilder<T>
    {
        IBuilder<T> Where(Action<T> action);
        T Build();
    }
}

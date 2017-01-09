using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;

namespace MPE.SS.Interfaces
{
    public interface IDataCollector<T>
    {
        string Name { get; }
        int SleepIntervalInMils { get; }
        void Collect(T server);
    }
}

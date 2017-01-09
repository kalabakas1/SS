using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;

namespace MPE.SS.Interfaces
{
    internal interface IBaselineService
    {
        void UpdateOrCreateBaseline(string server, string chartAlias,
            DateTime dataCollectionTimeStamp, long value);

        Baseline GetBaseline(string server, string chartAlias);
    }
}

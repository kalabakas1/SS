using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;

namespace MPE.SS.Interfaces
{
    internal interface IApplicationConfiguration
    {
        DateTime StartedAt { get; }
        AppState AppState { get; }
        bool CureTestIssues { get; }
        int DataCollectionIntervalMin { get; }
        int ChartUpdateIntervalMin { get; }
        int FileGarbageCollectorIntevalMin { get; }
        int DeleteDataFilesByAgeInMin { get; }
        int DeleteReportByAgeInMin { get; }
        int TestExecutionIntervalInMin { get; }
        int BaselineRenderingIntervalInMin { get; }
        int BaselineShiftInPercentage { get; }
    }
}

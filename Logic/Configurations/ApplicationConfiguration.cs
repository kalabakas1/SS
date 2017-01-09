using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;

namespace MPE.SS.Logic.Configurations
{
    internal class ApplicationConfiguration : IApplicationConfiguration
    {
        public ApplicationConfiguration()
        {
            StartedAt = DateTime.Now;
        }
        public AppState AppState { get { return AppState.Release; } }
        public bool CureTestIssues { get { return true; } }
        public int DataCollectionIntervalMin { get { return 60; } }
        public int ChartUpdateIntervalMin { get { return 1; } }
        public int FileGarbageCollectorIntevalMin { get { return 10; } }
        public int DeleteDataFilesByAgeInMin { get { return 60; } }
        public int DeleteReportByAgeInMin { get { return 1440; } }
        public int TestExecutionIntervalInMin { get { return 60; } }
        public int BaselineRenderingIntervalInMin { get { return 1440; } }
        public int BaselineShiftInPercentage { get { return 10; } }
        public DateTime StartedAt { get; }
    }
}

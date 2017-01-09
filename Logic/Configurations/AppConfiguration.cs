using MPE.SS.Interfaces;
using NLog;

namespace MPE.SS.Logic.Configurations
{
    internal class AppConfiguration
    {
        public static IApplicationConfiguration Configuration { get; set; }

        public static Logger Logger = LogManager.GetCurrentClassLogger();
    }
}

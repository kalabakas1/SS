using System.Collections.Generic;
using MPE.SS.Models;

namespace MPE.SS.Interfaces
{
    internal interface IConfigurationService
    {
        Configuration GetConfiguration(string alias);

        List<string> GetConfigurationAliases();
    }
}

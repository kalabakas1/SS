using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPE.SS.Interfaces
{
    internal interface IFileService
    {
        string GetEmbeddedRessource(string path);

        void SaveReport(string alias, string content);

        void DeleteOldFiles(string folder, int maxAgeInMin);
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;

namespace MPE.SS.Logic
{
    internal class FileService : IFileService
    {
        public string GetEmbeddedRessource(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using (Stream stream = assembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public void SaveReport(string alias, string content)
        {
            EnsureFolder(Constants.SystemConstant.ReportFolder);

            var historicFileName = string.Format("{0}_{1}_{2}", Constants.SystemConstant.ReportPrefix, alias,
                DateTime.Now.ToString("yyyyMMddHHmm"));

            File.WriteAllText(string.Format("./{0}/{1}.html", Constants.SystemConstant.ReportFolder, historicFileName), content);

            var path = string.Format("./{0}/{1}.html", Constants.SystemConstant.ReportFolder, alias);
            if(File.Exists(path))
                File.Delete(path);
            File.WriteAllText(path, content);
        }

        private void EnsureFolder(string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
        }

        public void DeleteOldFiles(string folder, int maxAgeInMin)
        {
            var files = Directory.GetFiles(folder);
            var fileInfos = files.Select(x => new FileInfo(x)).Where(x => x.LastWriteTime < DateTime.Now.AddMinutes(-1 * maxAgeInMin));
            foreach (var file in fileInfos)
            {
                File.Delete(file.FullName);
            }
        }
    }
}

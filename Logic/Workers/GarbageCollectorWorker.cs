using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MPE.SS.Constants;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;
using MPE.SS.Models.MongoDB;
using MPE.SS.Models.RedisInfo;
using MPE.SS.Models.ServerInfo;
using NLog;

namespace MPE.SS.Logic.Workers
{
    internal class GarbageCollectorWorker : IWorker
    {
        private IFileService _fileService;
        private IRepository<Report> _reportRepository;
        private IRepository<ServerInfo> _serverInfoRepository;
        private IRepository<RedisInfo> _redisiRepository;
        private IRepository<ServerUtilization> _serverUtilizationRepository;

        private Thread _garbageCollectorThread;
        public GarbageCollectorWorker(
            IFileService fileService,
            IRepository<Report> reportRepository,
            IRepository<ServerInfo> serverInfoRepository,
            IRepository<RedisInfo> redisInfoRepository,
            IRepository<ServerUtilization> serverUtilizationRepository)
        {
            _fileService = fileService;
            _reportRepository = reportRepository;
            _redisiRepository = redisInfoRepository;
            _serverInfoRepository = serverInfoRepository;
            _serverUtilizationRepository = serverUtilizationRepository;
        }

        public void Start()
        {
            _garbageCollectorThread = new Thread(() =>
            {
                while (true)
                {
                    try
                    {
                        _fileService.DeleteOldFiles(SystemConstant.ReportFolder, AppConfiguration.Configuration.DeleteReportByAgeInMin);
                        _fileService.DeleteOldFiles(Path.Combine(SystemConstant.DataFolder, typeof(Report).Name), AppConfiguration.Configuration.DeleteDataFilesByAgeInMin);
                        _fileService.DeleteOldFiles(Path.Combine(SystemConstant.DataFolder, typeof(ServerInfo).Name), AppConfiguration.Configuration.DeleteDataFilesByAgeInMin);
                        _fileService.DeleteOldFiles(Path.Combine(SystemConstant.DataFolder, typeof(RedisInfo).Name), AppConfiguration.Configuration.DeleteDataFilesByAgeInMin);
                        _fileService.DeleteOldFiles(Path.Combine(SystemConstant.DataFolder, typeof(MongoInfo).Name), AppConfiguration.Configuration.DeleteDataFilesByAgeInMin);
                        _fileService.DeleteOldFiles(Path.Combine(SystemConstant.DataFolder, typeof(ServerUtilization).Name), AppConfiguration.Configuration.DeleteDataFilesByAgeInMin);
                    }
                    catch (Exception e)
                    {
                        AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                    }
                    Thread.Sleep(AppConfiguration.Configuration.FileGarbageCollectorIntevalMin * 60000);
                }
            });
            _garbageCollectorThread.Start();
        }

        public void Stop()
        {
            try
            {
                _garbageCollectorThread.Abort();
            }
            catch { }
        }
    }
}

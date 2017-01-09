using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.ApplicationServices;
using MPE.SS.Constants;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Logic.Extensions;
using MPE.SS.Models;
using MPE.SS.Models.HaProxy;
using NLog;

namespace MPE.SS.Logic.DataCollectors
{
    internal class HaProxyCollector : IDataCollector<Server>
    {
        private object _lock = new object();

        private IRepository<HapInfo> _repository;
        private ICsvService<HapServer> _csvService;
        private IBuilder<HapInfo> _builder;

        public HaProxyCollector(
            IRepository<HapInfo> repository,
            ICsvService<HapServer> csvService,
            IBuilder<HapInfo> infoBuilder)
        {
            _repository = repository;
            _csvService = csvService;
            _builder = infoBuilder;
        }

        public string Name { get { return "HaProxyCollector"; } }

        public int SleepIntervalInMils { get { return 10000; } }

        public void Collect(Server server)
        {
            lock (_lock)
            {
                if (IsValid(server))
                {
                    try
                    {
                        var url = $"{server.Additionals[AdditionalConstant.HaProxyUrl]};csv";
                        var username = server.Additionals[AdditionalConstant.HaProxyUsername].ToString();
                        var password = server.Additionals[AdditionalConstant.HaProxyPassword].ToString();

                        var req = (HttpWebRequest)WebRequest.Create(url);
                        req.Credentials = new NetworkCredential(username, password);
                        var objs = new List<HapServer>();

                        using (var resp = req.GetResponse())
                        {
                            using (var rs = new StreamReader(resp.GetResponseStream()))
                            {
                                var content = rs.ReadToEnd().Split('\n');
                                var lines = content.Skip(1).ToList();

                                foreach (var line in lines)
                                {
                                    objs.Add(_csvService.ParseLine(line));
                                }
                            }
                        }

                        var info = _builder
                            .Where(x => x.Reference = server.Name)
                            .Where(x => x.ProxyNames = objs.Select(s => s.ProxyName).Where(z => !string.IsNullOrEmpty(z)).Distinct().ToList())
                            .Where(x => x.Servers = objs)
                            .Build();

                        server.CollectedData.AddOrUpdate(DataCollector.HaProxyInfo, info);

                        _repository.Save(info);
                    }
                    catch (Exception e)
                    {
                        AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                    }
                }
            }
        }

        private bool IsValid(Server server)
        {
            return server.Additionals.ContainsKey(AdditionalConstant.HaProxyUrl)
                  && server.Additionals.ContainsKey(AdditionalConstant.HaProxyUsername)
                  && server.Additionals.ContainsKey(AdditionalConstant.HaProxyPassword);
        }
    }
}

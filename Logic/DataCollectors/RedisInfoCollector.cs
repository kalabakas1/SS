using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Attributes;
using MPE.SS.Logic.Extensions;
using MPE.SS.Models;
using MPE.SS.Models.RedisInfo;

namespace MPE.SS.Logic.DataCollectors
{
    internal class RedisInfoCollector : IDataCollector<Server>
    {
        private PowerShellService _shellService;
        private IRepository<RedisInfo> _redisInfoRepository;

        public RedisInfoCollector(
            IRepository<RedisInfo> redisInfoRepository)
        {
            _redisInfoRepository = redisInfoRepository;
        }

        public string Name { get { return "RedisInfoCollector"; } }

        public int SleepIntervalInMils { get { return 30000; } }

        public void Collect(Server server)
        {
            if (IsValidRedisServer(server))
            {
                _shellService = new PowerShellService(server.Name, server.Credential.Username, server.Credential.Password);

                _shellService.Invoke(shell =>
                {
                    var name = server.Name;
                    var port = server.GetAdditional<int>(Constants.AdditionalConstant.RedisPort);
                    var password = server.GetAdditional<string>(Constants.AdditionalConstant.RedisPassword);
                    shell.AddScript(string.Format("redis-cli.exe -h {0} -p {1} -a {2} INFO",
                        name,
                        port,
                        password));
                    var results = shell.Invoke();
                    var info = ConvertInfoFromRaw(results.ToList());

                    shell.Commands.Clear();
                    shell.AddScript(string.Format("redis-cli.exe -h {0} -p {1} -a {2} CLIENT LIST",
                        name,
                        port,
                        password));
                    results = shell.Invoke();
                    ConvertClientList(results.ToList(), info);

                    if (info != null)
                    {
                        info.Reference = server.Name;
                        _redisInfoRepository.Save(info);
                        server.CollectedData.AddOrUpdate(Enums.DataCollector.RedisInfo, info);
                    }
                });
            }
        }

        private bool IsValidRedisServer(Server server)
        {
            return server.Services != null
                   && server.Services.Contains("Redis")
                   && server.Additionals.ContainsKey(Constants.AdditionalConstant.RedisPassword)
                   && server.Additionals.ContainsKey(Constants.AdditionalConstant.RedisPort);
        }

        private void ConvertClientList(List<PSObject> objects, RedisInfo info)
        {
            info.Clients = new List<RedisClient>();
            foreach (var obj in objects)
            {
                var strData = obj.ToString();
                var data = strData.Split(' ').Select(x =>
                {
                    var fieldData = x.Split('=');
                    return new KeyValuePair<string, string>(fieldData[0].Trim(), fieldData[1].Trim());
                }).ToDictionary(x => x.Key, x => x.Value);
                var client = new RedisClient();
                Map(data, client);

                info.Clients.Add(client);
            }
        }

        private RedisInfo ConvertInfoFromRaw(List<PSObject> objects)
        {
            var info = new RedisInfo
            {
                Server = new RedisServerInfo(),
                Statistic = new RedisStatisticInfo(),
                Persistance = new RedisPersistanceInfo(),
                Client = new RedisClientInfo(),
                Cpu = new RedisCpuInfo(),
                Memory = new RedisMemoryInfo(),
                Replication = new RedisReplicationInfo()
            };

            var raw = new Dictionary<string, string>();
            foreach (var psObject in objects)
            {
                var line = psObject.ToString().Trim();
                if (!string.IsNullOrEmpty(line)
                    && !line.StartsWith("#"))
                {
                    var data = line.Split(':');
                    raw.Add(data[0].Trim(), string.Join(":", data.Skip(1)));
                }
            }

            Map(raw, info.Server);
            Map(raw, info.Statistic);
            Map(raw, info.Persistance);
            Map(raw, info.Client);
            Map(raw, info.Cpu);
            Map(raw, info.Memory);
            Map(raw, info.Replication);

            return info;
        }

        private void Map(Dictionary<string, string> data, object obj)
        {
            var properties = obj.GetType()
                .GetProperties()
                .Where(x => x.GetCustomAttributes<RedisFieldAttribute>().Any());
            foreach (var property in properties)
            {
                var key = property.GetCustomAttribute<RedisFieldAttribute>().RedisFieldName;
                if (data.ContainsKey(key))
                {
                    var value = data[key];
                    try
                    {
                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(obj, convertedValue);
                    }
                    catch
                    {

                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using Newtonsoft.Json;

namespace MPE.SS.Models
{
    public class Server
    {
        public Server()
        {
            CollectedData = CollectedData ?? new Dictionary<DataCollector, Data<Server>>();
            Additionals = Additionals ?? new Dictionary<string, object>();
        }

        public string CredentialAlias { get; set; }
        public string Label { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
        public string[] AppPools { get; set; }
        public string[] Services { get; set; }
        public string[] ScheduledTasks { get; set; }
        public Connection[] RequiredConnections { get; set; }
        public string[] Features { get; set; }
        public List<string> Ignore { get; set; }
        public Dictionary<string,object> Additionals { get; set; }

        public List<Alert> Alerts { get; set; }

        [JsonIgnore]
        public Credential Credential { get; set; }

        public Dictionary<DataCollector, Data<Server>> CollectedData { get; set; }

        public bool IgnoreStep(string alias)
        {
            if (Ignore == null)
                return false;
            return Ignore.Contains(alias);
        }

        public T GetAdditional<T>(string key)
        {
            if (Additionals.ContainsKey(key))
            {
                try
                {
                    return (T)Convert.ChangeType(Additionals[key], typeof(T));
                }
                catch
                {
                    return default(T);
                }
            }
            return default(T);
        }
    }
}

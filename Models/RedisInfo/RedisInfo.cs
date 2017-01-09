using System.Collections.Generic;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisInfo : Data<Server>
    {
        public RedisServerInfo Server { get; set; }
        public RedisClientInfo Client { get; set; }
        public RedisMemoryInfo Memory { get; set; }
        public RedisPersistanceInfo Persistance { get; set; }
        public RedisStatisticInfo Statistic { get; set; }
        public RedisReplicationInfo Replication { get; set; }
        public RedisCpuInfo Cpu { get; set; }
        public List<RedisClient> Clients { get; set; }
    }
}

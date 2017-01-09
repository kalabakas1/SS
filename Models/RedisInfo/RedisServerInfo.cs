using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisServerInfo
    {
        [RedisField("redis_version")]
        public string Version { get; set; }
        [RedisField("redis_build_id")]
        public string BuildId { get; set; }
        [RedisField("redis_mode")]
        public string Mode { get; set; }
        [RedisField("os")]
        public string OS { get; set; }
        [RedisField("arch_bits")]
        public int ArchitectureBits { get; set; }
        [RedisField("multiplexing_api")]
        public string MultiplexingApi { get; set; }
        [RedisField("process_id")]
        public string ProcessId { get; set; }
        [RedisField("run_id")]
        public string RunId { get; set; }
        [RedisField("tcp_port")]
        public int TcpPort { get; set; }
        [RedisField("uptime_in_seconds")]
        public long UptimeInSec { get; set; }
        [RedisField("uptime_in_days")]
        public long UptimeInDays { get; set; }
        [RedisField("hz")]
        public int Hz { get; set; }
        [RedisField("executable")]
        public string Executable { get; set; }
        [RedisField("config_file")]
        public string ConfigFile { get; set; }
    }
}
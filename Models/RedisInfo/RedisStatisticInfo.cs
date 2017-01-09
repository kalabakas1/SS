using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisStatisticInfo
    {
        [RedisField("total_connections_received")]
        public long TotalConnectionsReceived { get; set; }
        [RedisField("total_commands_processed")]
        public long TotalCommandProcessed { get; set; }
        [RedisField("instantaneous_ops_per_sec")]
        public long InstantaneousOpsPerSec { get; set; }
        [RedisField("total_net_input_bytes")]
        public long TotalNetInputBytes { get; set; }
        [RedisField("total_net_output_bytes")]
        public long TotalNetOutputBytes { get; set; }
        [RedisField("instantaneous_input_kbps")]
        public double InstantaneousInputKbps { get; set; }
        [RedisField("instantaneous_output_kbps")]
        public double InstantaneousOutputKbps { get; set; }
        [RedisField("rejected_connections")]
        public long RejectedConnections { get; set; }
        [RedisField("sync_full")]
        public int SyncFull { get; set; }
        [RedisField("sync_partial_ok")]
        public int SyncPartialOk { get; set; }
        [RedisField("sync_partial_err")]
        public long SyncPartialError { get; set; }
        [RedisField("expired_keys")]
        public long ExpiredKeys { get; set; }
        [RedisField("evicted_keys")]
        public long EvictedKeys { get; set; }
        [RedisField("keyspace_hits")]
        public long KeyspaceHits { get; set; }
        [RedisField("keyspace_misses")]
        public long KeyspaceMisses { get; set; }
        [RedisField("pubsub_channels")]
        public long PubSubChannels { get; set; }
        [RedisField("pubsub_patterns")]
        public long PubSubPatterns { get; set; }
        [RedisField("latest_fork_usec")]
        public long LatestForkUsec { get; set; }
        [RedisField("migrate_cached_sockets")]
        public long MigratedCachedSockets { get; set; }
    }
}
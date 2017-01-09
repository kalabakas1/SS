using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisReplicationInfo
    {
        [RedisField("role")]
        public string Role { get; set; }
        [RedisField("connected_slaves")]
        public int ConnectedSlaves { get; set; }
        [RedisField("master_repl_offset")]
        public int MasterReplicationOffset { get; set; }
        [RedisField("repl_backlog_active")]
        public string ReplicationBacklogActive { get; set; }
        [RedisField("repl_backlog_size")]
        public long ReplicationBacklogSize { get; set; }
        [RedisField("repl_backlog_first_byte_offset")]
        public long ReplicationBacklogFirstByteOffset { get; set; }
        [RedisField("repl_backlog_histlen")]
        public long ReplicationBacklogHistoryLength { get; set; }
    }
}
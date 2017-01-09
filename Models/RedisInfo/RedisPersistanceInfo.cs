using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisPersistanceInfo
    {
        [RedisField("loading")]
        public int Loading { get; set; }
        [RedisField("rdb_changes_since_last_save")]
        public int ChangesSinceLastSave { get; set; }
        [RedisField("rdb_bgsave_in_progress")]
        public int BackgroundSaveInProgress { get; set; }
        [RedisField("rdb_last_save_time")]
        public long LastSaveTime { get; set; }
        [RedisField("rdb_last_bgsave_status")]
        public string LastBackgroundSaveStatus { get; set; }
        [RedisField("rdb_last_bgsave_time_sec")]
        public long LastBackgroundSaveTimeSec { get; set; }
        [RedisField("rdb_current_bgsave_time_sec")]
        public long CurrentBackgroundSaveTimeSec { get; set; }
    }
}
using MPE.SS.Logic.Attributes;

namespace MPE.SS.Models.RedisInfo
{
    public class RedisClientInfo
    {
        [RedisField("connected_clients")]
        public int Connected { get; set; }
        [RedisField("client_longest_output_list")]
        public int LongestOutputList { get; set; }
        [RedisField("client_biggest_input_buf")]
        public int BiggestInputBuf { get; set; }
        [RedisField("blocked_clients")]
        public int Blocked { get; set; }
    }
}
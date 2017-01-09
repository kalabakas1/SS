using MongoDB.Bson.Serialization.Attributes;

namespace MPE.SS.Models.MongoDB
{
    public class MongoOpcounters
    {
        [BsonElement("insert")]
        public int Insert { get; set; }
        [BsonElement("query")]
        public int Query { get; set; }
        [BsonElement("update")]
        public int Update { get; set; }
        [BsonElement("delete")]
        public int Delete { get; set; }
        [BsonElement("getmore")]
        public int Getmore { get; set; }
        [BsonElement("command")]
        public int Command { get; set; }
    }
}
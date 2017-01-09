using MongoDB.Bson.Serialization.Attributes;

namespace MPE.SS.Models.MongoDB
{
    public class MongoConnections
    {
        [BsonElement("current")]
        public int Current { get; set; }
        [BsonElement("available")]
        public int Available { get; set; }
        [BsonElement("totalCreated")]
        public long TotalCreated { get; set; }
    }
}
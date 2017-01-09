using MongoDB.Bson.Serialization.Attributes;

namespace MPE.SS.Models.MongoDB
{
    public class MongoNetwork
    {
        [BsonElement("bytesIn")]
        public long BytesIn { get; set; }
        [BsonElement("bytesOut")]
        public long BytesOut { get; set; }
        [BsonElement("numRequests")]
        public long NumRequests { get; set; }
    }
}
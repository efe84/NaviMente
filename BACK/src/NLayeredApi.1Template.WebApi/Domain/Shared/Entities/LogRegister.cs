using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class LogRegister
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }

        [BsonElement("FileName")]
        public string? FileName { get; set; }

        [BsonElement("Timestamp")]
        public DateTime? Timestamp { get; set; }

        [BsonElement("Content")]
        public List<LogEvent>? Content { get; set; }

        [BsonElement("LastUpdate")]
        public DateTime? LastUpdate { get; set; }
    }
}

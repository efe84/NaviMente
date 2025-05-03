using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class LogLine
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }

        [BsonElement("serialNumber")]
        public required string SerialNumber { get; set; }

        [BsonElement("timestamp")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Timestamp { get; set; }

        [BsonElement("severity")]
        public required long Severity { get; set; }

        [BsonElement("message")]
        public string? Message { get; set; }
    }
}

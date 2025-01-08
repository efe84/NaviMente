using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class LogEvent
    {
        [BsonElement("Date")]
        public DateTime? Date { get; set; }

        [BsonElement("Severity")]
        public string? Severity { get; set; }

        [BsonElement("Event")]
        public string? Event { get; set; }
    }
}

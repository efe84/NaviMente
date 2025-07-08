using MongoDB.Bson.Serialization.Attributes;
using NaviMente.WebApi.Dto.Enums;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class Counter
    {
        [BsonId]
        public required string Id { get; set; }

        [BsonElement("sequenceValue")]
        public required long SequenceValue { get; set; }
    }
}

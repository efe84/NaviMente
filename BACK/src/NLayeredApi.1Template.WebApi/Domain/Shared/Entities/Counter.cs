using MongoDB.Bson.Serialization.Attributes;
using NaviMente.WebApi.Dto.Enums;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class Counter
    {
        [BsonElement("sequenceValue")]
        public long? UserId { get; set; }
    }
}

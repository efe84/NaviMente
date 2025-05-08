using MongoDB.Bson.Serialization.Attributes;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class TelegramLinkCode
    {
        [BsonElement("code")]
        public required string Code { get; set; }

        [BsonElement("userId")]
        public required long UserId { get; set; }

        [BsonElement("expiresAt")]
        public DateTime ExpiresAt { get; set; }
    }
}

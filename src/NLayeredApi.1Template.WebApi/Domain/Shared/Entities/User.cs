using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UserId { get; set; }

        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("phone")]
        public string? PhoneNumber { get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NaviMente.WebApi.Dto.Enums;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? Id { get; set; }

        [BsonElement("userId")]
        public long? UserId { get; set; }

        [BsonElement("username")]
        public string? Username { get; set; }

        [BsonElement("email")]
        public string? Email { get; set; }

        [BsonElement("password")]
        public string? Password { get; set; }

        [BsonElement("mainPhone")]
        public string? MainPhone { get; set; }
        
        [BsonElement("otherPhones")]
        public List<string>? OtherPhones { get; set; }

        [BsonElement("role")]
        public UserRolesEnum Role {  get; set; }
    }
}

using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using NaviMente.WebApi.Dto.Enums;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId? DeviceId { get; set; }

        [BsonElement("deviceName")]
        public string? DeviceName { get; set; }

        [BsonElement("serialNumber")]
        public string? SerialNumber { get; set; }

        [BsonElement("userId")]
        public long? UserId { get; set; }

        [BsonElement("assignedDate")]
        public DateTime? AssignedDate { get; set; }

        [BsonElement("isActive")]
        public Boolean? isActive { get; set; }
    }
}

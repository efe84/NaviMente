using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class Location
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("serialNumber")]
        public string? SerialNumber { get; set; }

        [BsonElement("location")]
        public GeoJsonGeometry<GeoJson2DCoordinates>? LocationData { get; set; }

        [BsonElement("timestamp")]
        [BsonRepresentation(BsonType.DateTime)]
        public DateTime Timestamp { get; set; }
    }
}

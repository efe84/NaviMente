using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class RestrictedZone
    {
        [BsonId]
        public ObjectId ZoneId { get; set; }

        [BsonElement("serialNumber")]
        public required string SerialNumber { get; set; }

        [BsonElement("shapes")]
        public required List<GeoJsonPolygon<GeoJson2DGeographicCoordinates>> Shapes { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }
    }
}

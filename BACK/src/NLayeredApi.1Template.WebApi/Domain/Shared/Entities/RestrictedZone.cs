using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using NaviMente.WebApi.Dto.Device;

namespace NaviMente.WebApi.Domain.Shared.Entities
{
    public class RestrictedZone
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public required long ZoneId { get; set; }
        public required string SerialNumber { get; set; }
        public required string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public GeoJson2DGeographicCoordinates? Center { get; set; }
        public double? Radius { get; set; }
        public BoundsDTO? Bounds { get; set; }
        public GeoJsonPolygon<GeoJson2DGeographicCoordinates>? Shape { get; set; }
    }
}

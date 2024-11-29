using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace NaviMente.WebApi.Dto.Location
{
    public class LocationPointDTO
    {
        public long? DeviceId { get; set; }
        public double? Longitude { get; set; }
        public double? Latitude { get; set; }
        public DateTime Timestamp { get; set; }
    }
}

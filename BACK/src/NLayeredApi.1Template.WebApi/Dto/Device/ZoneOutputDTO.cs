using MongoDB.Bson;

namespace NaviMente.WebApi.Dto.Device
{
    public class ZoneOutputDTO
    {
        public required long ZoneId { get; set; }
        public required string SerialNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public required ShapeOutputDTO Shape { get; set; }
    }

    public class ShapeOutputDTO
    {
        public required string Type { get; set; }

        public List<List<double>>? Coordinates { get; set; }

        public List<double>? Center { get; set; }

        public double? Radius { get; set; }

        public BoundsDTO? Bounds { get; set; }
    }

}

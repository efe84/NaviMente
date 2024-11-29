using MongoDB.Driver.GeoJsonObjectModel;

namespace NaviMente.WebApi.Dto.Location
{
    public class LocationLineDTO
    {
        public long DeviceId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Route? Route { get; set; }
    }

    public class Route
    {
        public string? Type { get; set; } = "LineString";
        public List<List<double>?> Coordinates { get; set; } = new List<List<double>?>();
    }

}

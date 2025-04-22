namespace NaviMente.WebApi.Dto.Device
{
    public class ZoneDTO
    {
        public required string SerialNumber { get; set; }
        public required List<PolygonDTO> Shapes { get; set; }
    }

    public class PolygonDTO
    {
        public string Type { get; set; } = "Polygon";
        public required List<List<double>>[] Coordinates { get; set; }
    }
}

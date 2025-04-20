namespace NaviMente.WebApi.Dto.Device
{
    public class ZoneOutputDTO
    {
        public required string SerialNumber { get; set; }
        public List<ShapeDto>? Shapes { get; set; }
    }

    public class ShapeDto
    {
        public required string Type { get; set; }
        public List<List<CoordinateDto>>? Coordinates { get; set; }
    }

    public class CoordinateDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

}

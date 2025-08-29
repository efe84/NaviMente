namespace NaviMente.WebApi.Dto.Device
{
    public class ZoneDTO
    {
        public required string SerialNumber { get; set; }
        public required List<ShapeDTO> Shapes { get; set; }
    }

    public class ShapeDTO
    {
        public required string Type { get; set; }
        public double Radius { get; set; }
        public CoordinateDTO? Center { get; set; }
        public BoundsDTO? Bounds { get; set; }
        public List<List<double>>? Coordinates { get; set; }
    }

    public class CoordinateDTO
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class BoundsDTO
    {
        public double North { get; set; }
        public double South { get; set; }
        public double East { get; set; }
        public double West { get; set; }
    }
}

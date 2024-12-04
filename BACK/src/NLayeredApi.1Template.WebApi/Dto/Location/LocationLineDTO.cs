﻿using MongoDB.Driver.GeoJsonObjectModel;

namespace NaviMente.WebApi.Dto.Location
{
    public class LocationLineDTO
    {
        public string? SerialNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Route>? Routes { get; set; }
    }

    public class Route
    {
        public string? Type { get; set; } = "LineString";
        public List<List<double>?> Coordinates { get; set; } = new List<List<double>?>();
    }

}

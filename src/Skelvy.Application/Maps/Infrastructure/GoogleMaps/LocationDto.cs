using System;

namespace Skelvy.Application.Maps.Infrastructure.GoogleMaps
{
  [Serializable]
  public class LocationDto
  {
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string Type { get; set; }
  }
}

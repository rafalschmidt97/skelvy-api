using System;

namespace Skelvy.Application.Maps.Infrastructure.GoogleMaps
{
  [Serializable]
  public class Location
  {
    public Location(double latitude, double longitude, string country, string state, string district, string city, string type)
    {
      Latitude = latitude;
      Longitude = longitude;
      Country = country;
      State = state;
      District = district;
      City = city;
      Type = type;
    }

    public Location()
    {
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Country { get; set; }
    public string State { get; set; }
    public string District { get; set; }
    public string City { get; set; }
    public string Type { get; set; }
  }
}

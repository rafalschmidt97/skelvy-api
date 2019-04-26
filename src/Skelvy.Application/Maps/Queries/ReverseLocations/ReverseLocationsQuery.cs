using System.Collections.Generic;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQuery : IQuery<IList<LocationDto>>
  {
    public ReverseLocationsQuery(double latitude, double longitude, string language)
    {
      Latitude = latitude;
      Longitude = longitude;
      Language = language;
    }

    public ReverseLocationsQuery() // required for FromQuery attribute
    {
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Language { get; set; }
  }
}

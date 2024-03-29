using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Domain.Enums;

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

    [JsonConstructor]
    public ReverseLocationsQuery()
    {
    }

    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

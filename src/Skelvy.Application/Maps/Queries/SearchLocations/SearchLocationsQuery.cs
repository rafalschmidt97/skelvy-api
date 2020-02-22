using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQuery : IQuery<IList<LocationDto>>
  {
    public SearchLocationsQuery(string search, string language)
    {
      Search = search;
      Language = language;
    }

    [JsonConstructor]
    public SearchLocationsQuery()
    {
    }

    public string Search { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

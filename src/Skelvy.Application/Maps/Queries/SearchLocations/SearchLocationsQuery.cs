using System.Collections.Generic;
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

    public SearchLocationsQuery() // required for FromQuery attribute
    {
    }

    public string Search { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

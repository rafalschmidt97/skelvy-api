using System.Collections.Generic;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQuery : IQuery<IList<Location>>
  {
    public string Search { get; set; }
    public string Language { get; set; }
  }
}

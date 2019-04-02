using System.Collections.Generic;
using MediatR;
using Skelvy.Application.Infrastructure.Maps;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQuery : IRequest<IList<Location>>
  {
    public string Search { get; set; }
    public string Language { get; set; }
  }
}

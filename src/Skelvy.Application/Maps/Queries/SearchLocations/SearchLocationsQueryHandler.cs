using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Core.Cache;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQueryHandler : QueryHandler<SearchLocationsQuery, IList<LocationDto>>
  {
    private readonly IMapsService _mapsService;
    private readonly ICacheService _cache;

    public SearchLocationsQueryHandler(IMapsService mapsService, ICacheService cache)
    {
      _mapsService = mapsService;
      _cache = cache;
    }

    public override async Task<IList<LocationDto>> Handle(SearchLocationsQuery request)
    {
      return await _cache.GetOrSetData(
        $"maps:search#{request.Search}#{request.Language}",
        TimeSpan.FromDays(14),
        async () => await _mapsService.Search(request.Search, request.Language));
    }
  }
}

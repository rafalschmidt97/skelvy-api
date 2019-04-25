using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Core.Cache;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQueryHandler : QueryHandler<ReverseLocationsQuery, IList<LocationDto>>
  {
    private readonly IMapsService _mapsService;
    private readonly ICacheService _cache;

    public ReverseLocationsQueryHandler(IMapsService mapsService, ICacheService cache)
    {
      _mapsService = mapsService;
      _cache = cache;
    }

    public override async Task<IList<LocationDto>> Handle(ReverseLocationsQuery request)
    {
      return await _cache.GetOrSetData(
        $"maps:reverse#{request.Latitude}#{request.Longitude}#{request.Language}",
        TimeSpan.FromDays(14),
        async () => await _mapsService.Search(request.Latitude, request.Longitude, request.Language));
    }
  }
}

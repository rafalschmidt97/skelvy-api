using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Maps.Infrastructure.GoogleMaps;
using Skelvy.Common.Serializers;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQueryHandler : QueryHandler<ReverseLocationsQuery, IList<LocationDto>>
  {
    private readonly IMapsService _mapsService;
    private readonly IDistributedCache _cache;

    public ReverseLocationsQueryHandler(IMapsService mapsService, IDistributedCache cache)
    {
      _mapsService = mapsService;
      _cache = cache;
    }

    public override async Task<IList<LocationDto>> Handle(ReverseLocationsQuery request)
    {
      var cacheKey = $"maps:reverse#{request.Latitude}#{request.Longitude}#{request.Language}";
      var cachedLocationBytes = await _cache.GetAsync(cacheKey);

      if (cachedLocationBytes != null)
      {
        return cachedLocationBytes.Deserialize<IList<LocationDto>>();
      }

      var locations = await _mapsService.Search(request.Latitude, request.Longitude, request.Language);

      var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(14));
      await _cache.SetAsync(cacheKey, locations.Serialize(), options);

      return locations;
    }
  }
}

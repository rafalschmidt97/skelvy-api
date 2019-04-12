using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Infrastructure.Maps;
using Skelvy.Common.Serializers;

namespace Skelvy.Application.Maps.Queries.SearchLocations
{
  public class SearchLocationsQueryHandler : QueryHandler<SearchLocationsQuery, IList<Location>>
  {
    private readonly IMapsService _mapsService;
    private readonly IDistributedCache _cache;

    public SearchLocationsQueryHandler(IMapsService mapsService, IDistributedCache cache)
    {
      _mapsService = mapsService;
      _cache = cache;
    }

    public override async Task<IList<Location>> Handle(SearchLocationsQuery request)
    {
      var cacheKey = $"maps:search#{request.Search}#{request.Language}";
      var cachedLocationBytes = await _cache.GetAsync(cacheKey);

      if (cachedLocationBytes != null)
      {
        return cachedLocationBytes.Deserialize<IList<Location>>();
      }

      var locations = await _mapsService.Search(request.Search, request.Language);

      var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(14));
      await _cache.SetAsync(cacheKey, locations.Serialize(), options);

      return locations;
    }
  }
}

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using Skelvy.Application.Infrastructure.Maps;
using Skelvy.Common.Serializers;

namespace Skelvy.Application.Maps.Queries.ReverseLocations
{
  public class ReverseLocationsQueryHandler : IRequestHandler<ReverseLocationsQuery, ICollection<Location>>
  {
    private readonly IMapsService _mapsService;
    private readonly IDistributedCache _cache;

    public ReverseLocationsQueryHandler(IMapsService mapsService, IDistributedCache cache)
    {
      _mapsService = mapsService;
      _cache = cache;
    }

    public async Task<ICollection<Location>> Handle(ReverseLocationsQuery request, CancellationToken cancellationToken)
    {
      var cacheKey = $"maps:reverse#{request.Latitude}#{request.Longitude}#{request.Language}";
      var cachedLocationBytes = await _cache.GetAsync(cacheKey, cancellationToken);

      if (cachedLocationBytes != null)
      {
        return cachedLocationBytes.Deserialize<ICollection<Location>>();
      }

      var locations = await _mapsService.Search(request.Latitude, request.Longitude, request.Language, cancellationToken);

      var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(7));
      await _cache.SetAsync(cacheKey, locations.Serialize(), options, cancellationToken);

      return locations;
    }
  }
}

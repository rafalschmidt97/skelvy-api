using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Skelvy.Application.Core.Cache;
using Skelvy.Common.Serializers;

namespace Skelvy.Infrastructure.Core
{
  public class CacheService : ICacheService
  {
    private readonly IDistributedCache _cache;

    public CacheService(IDistributedCache cache)
    {
      _cache = cache;
    }

    public async Task<T> GetData<T>(string key)
    {
      var cachedBytes = await _cache.GetAsync(key);
      return cachedBytes.Deserialize<T>();
    }

    public async Task<T> GetOrSetData<T>(string key, TimeSpan expiration, Func<Task<T>> getFunction)
    {
      var cachedBytes = await _cache.GetAsync(key);

      if (cachedBytes != null)
      {
        return cachedBytes.Deserialize<T>();
      }

      var data = await getFunction();

      if (data != null)
      {
        var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiration);
        await _cache.SetAsync(key, data.Serialize(), options);
      }

      return data;
    }

    public async Task SetData(string key, TimeSpan expiration, object data)
    {
      var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiration);
      await _cache.SetAsync(key, data.Serialize(), options);
    }

    public async Task RefreshData(string key)
    {
      await _cache.RefreshAsync(key);
    }

    public async Task RemoveData(string key)
    {
      await _cache.RemoveAsync(key);
    }
  }
}

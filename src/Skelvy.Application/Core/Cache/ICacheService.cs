using System;
using System.Threading.Tasks;

namespace Skelvy.Application.Core.Cache
{
  public interface ICacheService
  {
    Task<T> GetData<T>(string key);
    Task<T> GetOrSetData<T>(string key, TimeSpan expiration, Func<Task<T>> getFunction);
    Task SetData(string key, TimeSpan expiration, object data);
    Task RefreshData(string key);
    Task RemoveData(string key);
  }
}

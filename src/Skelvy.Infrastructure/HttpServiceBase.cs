using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Skelvy.Infrastructure
{
  public abstract class HttpServiceBase
  {
    protected HttpServiceBase(string baseUrl)
    {
      HttpClient = new HttpClient
      {
        BaseAddress = new Uri(baseUrl),
      };
      HttpClient.DefaultRequestHeaders
        .Accept
        .Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    protected HttpClient HttpClient { get; }

    protected static async Task<T> GetData<T>(HttpContent responseContent)
    {
      var result = await responseContent.ReadAsStringAsync();
      return JsonConvert.DeserializeObject<T>(result, new JsonSerializerSettings
      {
        ContractResolver = new DefaultContractResolver
        {
          NamingStrategy = new SnakeCaseNamingStrategy(),
        },
        Formatting = Formatting.Indented,
      });
    }

    protected static StringContent PrepareData(object data)
    {
      var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
      {
        ContractResolver = new DefaultContractResolver
        {
          NamingStrategy = new SnakeCaseNamingStrategy(),
        },
        Formatting = Formatting.Indented,
      });
      return new StringContent(json, Encoding.UTF8, "application/json");
    }
  }
}

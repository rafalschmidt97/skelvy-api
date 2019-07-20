using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Skelvy.Common.Serializers
{
  public static class JsonSerializerExtension
  {
    public static string JsonSerialize(this object objectToSerialize)
    {
      return JsonConvert.SerializeObject(objectToSerialize, new JsonSerializerSettings
      {
        ContractResolver = new DefaultContractResolver
        {
          NamingStrategy = new CamelCaseNamingStrategy(),
        },
        Formatting = Formatting.Indented,
      });
    }

    public static T JsonDeserialize<T>(this string stringToDeserialize)
    {
      return JsonConvert.DeserializeObject<T>(stringToDeserialize, new JsonSerializerSettings
      {
        ContractResolver = new DefaultContractResolver
        {
          NamingStrategy = new CamelCaseNamingStrategy(),
        },
        Formatting = Formatting.Indented,
      });
    }
  }
}

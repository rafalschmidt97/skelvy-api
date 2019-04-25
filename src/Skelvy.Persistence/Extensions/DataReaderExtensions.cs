using System.Data;
using System.Linq;
using System.Runtime.Serialization;

namespace Skelvy.Persistence.Extensions
{
  public static class DataReaderExtensions
  {
    public static T Convert<T>(this IDataRecord source)
    {
      var destination = (T)FormatterServices.GetUninitializedObject(typeof(T));

      typeof(T).GetProperties()
        .ToList()
        .ForEach(property =>
        {
          try
          {
            var index = source.GetOrdinal(property.Name);
            var value = source.GetValue(index);
            property.SetValue(destination, System.Convert.ChangeType(value, property.PropertyType));
          }
          catch
          {
            // There is no way to check if index with a value exists. It catches IndexOutOfRangeException
          }
        });

      return destination;
    }
  }
}

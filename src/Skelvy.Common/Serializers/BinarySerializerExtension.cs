using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Skelvy.Common.Serializers
{
  public static class BinarySerializerExtension
  {
    public static byte[] Serialize(this object objectToSerialize)
    {
      if (objectToSerialize == null)
      {
        return null;
      }

      var binaryFormatter = new BinaryFormatter();
      using (var memoryStream = new MemoryStream())
      {
        binaryFormatter.Serialize(memoryStream, objectToSerialize);
        return memoryStream.ToArray();
      }
    }

    public static T Deserialize<T>(this byte[] bytesToDeserialize)
    {
      if (bytesToDeserialize == null)
      {
        return default(T);
      }

      var binaryFormatter = new BinaryFormatter();
      using (var memoryStream = new MemoryStream(bytesToDeserialize))
      {
        return (T)binaryFormatter.Deserialize(memoryStream);
      }
    }
  }
}

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Skelvy.Common.Serializers
{
  public static class BinarySerializerExtension
  {
    public static byte[] BinarySerialize(this object objectToSerialize)
    {
      if (objectToSerialize == null)
      {
        return null;
      }

      var binaryFormatter = new BinaryFormatter();
      var memoryStream = new MemoryStream();
      binaryFormatter.Serialize(memoryStream, objectToSerialize);
      return memoryStream.ToArray();
    }

    public static T BinaryDeserialize<T>(this byte[] bytesToDeserialize)
    {
      if (bytesToDeserialize == null)
      {
        return default;
      }

      var binaryFormatter = new BinaryFormatter();
      var memoryStream = new MemoryStream(bytesToDeserialize);
      return (T)binaryFormatter.Deserialize(memoryStream);
    }
  }
}

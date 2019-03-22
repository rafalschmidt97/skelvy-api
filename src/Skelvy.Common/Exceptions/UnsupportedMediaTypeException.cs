using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class UnsupportedMediaTypeException : CustomException
  {
    public UnsupportedMediaTypeException(string message)
      : base(HttpStatusCode.UnsupportedMediaType, message)
    {
    }

    public UnsupportedMediaTypeException()
      : base(HttpStatusCode.UnsupportedMediaType)
    {
    }
  }
}

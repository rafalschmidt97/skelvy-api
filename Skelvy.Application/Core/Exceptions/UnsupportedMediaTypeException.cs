using System.Net;

namespace Skelvy.Application.Core.Exceptions
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

using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class RequestTimeoutException : CustomException
  {
    public RequestTimeoutException(string message)
      : base(HttpStatusCode.RequestTimeout, message)
    {
    }

    public RequestTimeoutException()
      : base(HttpStatusCode.RequestTimeout)
    {
    }
  }
}

using System.Net;

namespace Skelvy.Common.Exceptions
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

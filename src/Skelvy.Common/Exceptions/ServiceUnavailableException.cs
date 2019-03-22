using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class ServiceUnavailableException : CustomException
  {
    public ServiceUnavailableException(string message)
      : base(HttpStatusCode.ServiceUnavailable, message)
    {
    }

    public ServiceUnavailableException()
      : base(HttpStatusCode.ServiceUnavailable)
    {
    }
  }
}

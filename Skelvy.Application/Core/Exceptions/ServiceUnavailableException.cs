using System.Net;

namespace Skelvy.Application.Core.Exceptions
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

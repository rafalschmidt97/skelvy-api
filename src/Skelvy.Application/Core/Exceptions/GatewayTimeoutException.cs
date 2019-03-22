using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class GatewayTimeoutException : CustomException
  {
    public GatewayTimeoutException(string message)
      : base(HttpStatusCode.Unauthorized, message)
    {
    }

    public GatewayTimeoutException()
      : base(HttpStatusCode.Unauthorized)
    {
    }
  }
}

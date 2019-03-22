using System.Net;

namespace Skelvy.Common.Exceptions
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

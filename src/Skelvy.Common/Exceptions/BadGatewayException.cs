using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class BadGatewayException : CustomException
  {
    public BadGatewayException(string message)
      : base(HttpStatusCode.BadGateway, message)
    {
    }

    public BadGatewayException()
      : base(nameof(HttpStatusCode.BadGateway))
    {
    }
  }
}

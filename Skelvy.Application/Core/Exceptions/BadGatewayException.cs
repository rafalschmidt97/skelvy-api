using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class BadGatewayException : CustomException
  {
    public BadGatewayException(string message)
      : base(HttpStatusCode.BadGateway, message)
    {
    }

    public BadGatewayException()
      : base(HttpStatusCode.BadGateway)
    {
    }
  }
}

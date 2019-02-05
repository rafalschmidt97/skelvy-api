using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class UnauthorizedException : CustomException
  {
    public UnauthorizedException(string message)
      : base(HttpStatusCode.Unauthorized, message)
    {
    }

    public UnauthorizedException()
      : base(HttpStatusCode.Unauthorized)
    {
    }
  }
}

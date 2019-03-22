using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class ForbiddenException : CustomException
  {
    public ForbiddenException(string message)
      : base(HttpStatusCode.Forbidden, message)
    {
    }

    public ForbiddenException()
      : base(HttpStatusCode.Forbidden)
    {
    }
  }
}

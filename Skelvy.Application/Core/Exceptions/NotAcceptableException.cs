using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class NotAcceptableException : CustomException
  {
    public NotAcceptableException(string message)
      : base(HttpStatusCode.NotAcceptable, message)
    {
    }

    public NotAcceptableException()
      : base(HttpStatusCode.NotAcceptable)
    {
    }
  }
}

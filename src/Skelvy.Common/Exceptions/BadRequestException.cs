using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class BadRequestException : CustomException
  {
    public BadRequestException(string message)
      : base(HttpStatusCode.BadRequest, message)
    {
    }

    public BadRequestException()
      : base(HttpStatusCode.BadRequest)
    {
    }
  }
}

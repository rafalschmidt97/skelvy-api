using System.Net;

namespace Skelvy.Application.Core.Exceptions
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

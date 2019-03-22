using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class InternalServerErrorException : CustomException
  {
    public InternalServerErrorException(string message)
      : base(HttpStatusCode.InternalServerError, message)
    {
    }

    public InternalServerErrorException()
      : base(HttpStatusCode.InternalServerError)
    {
    }
  }
}

using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class InternalServerErrorException : CustomException
  {
    public InternalServerErrorException(string message)
      : base(HttpStatusCode.InternalServerError, message)
    {
    }

    public InternalServerErrorException()
      : base(nameof(HttpStatusCode.InternalServerError))
    {
    }
  }
}

using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class UnprocessableEntityException : CustomException
  {
    public UnprocessableEntityException(string message)
      : base(HttpStatusCode.UnprocessableEntity, message)
    {
    }

    public UnprocessableEntityException()
      : base(HttpStatusCode.UnprocessableEntity)
    {
    }
  }
}

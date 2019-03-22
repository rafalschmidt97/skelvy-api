using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class ConflictException : CustomException
  {
    public ConflictException(string message)
      : base(HttpStatusCode.Conflict, message)
    {
    }

    public ConflictException()
      : base(HttpStatusCode.Conflict)
    {
    }
  }
}

using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class GoneException : CustomException
  {
    public GoneException(string message)
      : base(HttpStatusCode.Gone, message)
    {
    }

    public GoneException()
      : base(HttpStatusCode.Gone)
    {
    }
  }
}

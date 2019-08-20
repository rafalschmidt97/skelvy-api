using System.Net;

namespace Skelvy.Common.Exceptions
{
  public class NotFoundException : CustomException
  {
    public NotFoundException(string name, object key)
      : base(HttpStatusCode.NotFound, $"{name}({key}) not found.")
    {
    }

    public NotFoundException(string message)
      : base(HttpStatusCode.NotFound, message)
    {
    }

    public NotFoundException()
      : base(nameof(HttpStatusCode.NotFound))
    {
    }
  }
}

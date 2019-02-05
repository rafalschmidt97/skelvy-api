using System.Net;

namespace Skelvy.Application.Core.Exceptions
{
  public class NotFoundException : CustomException
  {
    public NotFoundException(string name, object key)
      : base(HttpStatusCode.NotFound, $"Entity {name}({key}) not found.")
    {
    }

    public NotFoundException(string message)
      : base(HttpStatusCode.NotFound, message)
    {
    }

    public NotFoundException()
      : base(HttpStatusCode.NotFound)
    {
    }
  }
}

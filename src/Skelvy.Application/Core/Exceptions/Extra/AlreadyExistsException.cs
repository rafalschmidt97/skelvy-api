namespace Skelvy.Application.Core.Exceptions.Extra
{
  public class AlreadyExistsException : ConflictException
  {
    public AlreadyExistsException(string name, object key)
      : base($"Entity {name}({key}) already exists.")
    {
    }
  }
}

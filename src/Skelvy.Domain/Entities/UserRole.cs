namespace Skelvy.Domain.Entities
{
  public class UserRole
  {
    public UserRole(string name, int userId)
    {
      Name = name;
      UserId = userId;
    }

    public UserRole(int id, string name, int userId)
      : this(name, userId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public int UserId { get; private set; }

    public User User { get; private set; }
  }
}

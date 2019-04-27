namespace Skelvy.Domain.Entities
{
  public class UserRole
  {
    public UserRole(string name, int userId)
    {
      Name = name;
      UserId = userId;
    }

    public UserRole(int id, string name, int userId, User user)
    {
      Id = id;
      Name = name;
      UserId = userId;
      User = user;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public int UserId { get; private set; }

    public User User { get; private set; }
  }
}

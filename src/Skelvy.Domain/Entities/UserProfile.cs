using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;

namespace Skelvy.Domain.Entities
{
  public class UserProfile : IModifiableEntity
  {
    public UserProfile(string name, DateTimeOffset birthday, string gender, int userId)
    {
      Name = name.Trim();
      Birthday = birthday;
      Gender = gender;
      UserId = userId;

      Photos = new List<UserProfilePhoto>();
    }

    public UserProfile(int id, string name, DateTimeOffset birthday, string gender, int userId)
      : this(name, birthday, gender, userId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public string Name { get; private set; }
    public DateTimeOffset Birthday { get; private set; }
    public string Gender { get; private set; }
    public string Description { get; private set; }
    public DateTimeOffset? ModifiedDate { get; private set; }
    public int UserId { get; private set; }

    public IList<UserProfilePhoto> Photos { get; private set; }
    public User User { get; private set; }

    public void Update(string name, DateTimeOffset birthday, string gender, string description)
    {
      Name = name.Trim();
      Birthday = birthday;
      Gender = gender;

      if (description != null)
      {
        Description = description.Trim();
      }

      ModifiedDate = DateTimeOffset.UtcNow;
    }
  }
}

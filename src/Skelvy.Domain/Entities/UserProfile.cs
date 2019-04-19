using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;

namespace Skelvy.Domain.Entities
{
  public class UserProfile : IModifiableEntity
  {
    public UserProfile()
    {
      Photos = new List<UserProfilePhoto>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    public int UserId { get; set; }

    public IList<UserProfilePhoto> Photos { get; private set; }
    public User User { get; set; }

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

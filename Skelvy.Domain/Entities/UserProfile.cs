using System;
using System.Collections.Generic;

namespace Skelvy.Domain.Entities
{
  public class UserProfile
  {
    public UserProfile()
    {
      Photos = new HashSet<UserProfilePhoto>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }

    public ICollection<UserProfilePhoto> Photos { get; private set; }
    public User User { get; set; }
  }
}

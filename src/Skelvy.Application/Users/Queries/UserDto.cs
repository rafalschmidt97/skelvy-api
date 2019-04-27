using System;
using System.Collections.Generic;

namespace Skelvy.Application.Users.Queries
{
  public class UserDto
  {
    public int Id { get; set; }
    public UserProfileDto Profile { get; set; }
  }

  public class UserProfileDto
  {
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<UserProfilePhotoDto> Photos { get; set; }
  }

  public class UserProfilePhotoDto
  {
    public string Url { get; set; }
  }
}

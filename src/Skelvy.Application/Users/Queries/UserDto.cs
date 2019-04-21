using System;
using System.Collections.Generic;

namespace Skelvy.Application.Users.Queries
{
  public class UserDto
  {
    public UserDto(int id, UserProfileDto profile)
    {
      Id = id;
      Profile = profile;
    }

    public UserDto()
    {
    }

    public int Id { get; }
    public UserProfileDto Profile { get; }
  }

  public class UserProfileDto
  {
    public UserProfileDto(string name, DateTimeOffset birthday, string gender, string description, IList<UserProfilePhotoDto> photos)
    {
      Name = name;
      Birthday = birthday;
      Gender = gender;
      Description = description;
      Photos = photos;
    }

    public UserProfileDto()
    {
    }

    public string Name { get; }
    public DateTimeOffset Birthday { get; }
    public string Gender { get; }
    public string Description { get; }
    public IList<UserProfilePhotoDto> Photos { get; }
  }

  public class UserProfilePhotoDto
  {
    public UserProfilePhotoDto(string url)
    {
      Url = url;
    }

    public string Url { get; }
  }
}

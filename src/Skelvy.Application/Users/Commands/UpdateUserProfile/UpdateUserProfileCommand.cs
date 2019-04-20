using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.UpdateUserProfile
{
  public class UpdateUserProfileCommand : ICommand
  {
    public UpdateUserProfileCommand(
      int userId,
      string name,
      DateTimeOffset birthday,
      string gender,
      IList<UpdateUserProfilePhotos> photos)
    {
      UserId = userId;
      Name = name;
      Birthday = birthday;
      Gender = gender;
      Photos = photos;
    }

    public UpdateUserProfileCommand(
      int userId,
      string name,
      DateTimeOffset birthday,
      string gender,
      string description,
      IList<UpdateUserProfilePhotos> photos)
      : this(userId, name, birthday, gender, photos)
    {
      Description = description;
    }

    public int UserId { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<UpdateUserProfilePhotos> Photos { get; set; }
  }

  public class UpdateUserProfilePhotos
  {
    public UpdateUserProfilePhotos(string url)
    {
      Url = url;
    }

    public string Url { get; set; }
  }
}

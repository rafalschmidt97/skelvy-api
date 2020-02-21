using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.UpdateProfile
{
  public class UpdateProfileCommand : ICommand
  {
    public UpdateProfileCommand(int userId, string name, DateTimeOffset birthday, string gender, string description, IList<UpdateProfilePhotos> photos)
    {
      UserId = userId;
      Name = name;
      Birthday = birthday;
      Gender = gender;
      Description = description;
      Photos = photos;
    }

    [JsonConstructor]
    public UpdateProfileCommand()
    {
    }

    public int UserId { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<UpdateProfilePhotos> Photos { get; set; }
  }

  public class UpdateProfilePhotos
  {
    public UpdateProfilePhotos(string url)
    {
      Url = url;
    }

    [JsonConstructor]
    public UpdateProfilePhotos()
    {
    }

    public string Url { get; set; }
  }
}

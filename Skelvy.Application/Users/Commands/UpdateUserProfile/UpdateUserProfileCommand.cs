using System;
using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Users.Commands.UpdateUserProfile
{
  public class UpdateUserProfileCommand : IRequest
  {
    public int UserId { get; set; }
    public string Name { get; set; }
    public DateTime Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public ICollection<UpdateUserProfilePhotos> Photos { get; set; }
  }

  public class UpdateUserProfilePhotos
  {
    public string Url { get; set; }
  }
}

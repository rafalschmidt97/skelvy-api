using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Skelvy.Application.Core.Mappers;
using Skelvy.Application.Users.Commands;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Queries
{
  public class UserDto
  {
    public int Id { get; set; }
    public UserProfileDto Profile { get; set; }
  }

  public class UserProfileDto : ICustomMapping
  {
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<UserProfilePhotoDto> Photos { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<UserProfile, UserProfileDto>()
        .ForMember(
          destination => destination.Photos,
          options => options
            .MapFrom(x => x.Photos.Where(y => y.Status == UserProfilePhotoStatusTypes.Active)));
    }
  }

  public class UserProfilePhotoDto
  {
    public string Url { get; set; }
  }
}

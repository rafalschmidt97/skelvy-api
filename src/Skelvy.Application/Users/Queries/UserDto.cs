using System;
using System.Collections.Generic;
using AutoMapper;
using Skelvy.Application.Core.Mappers;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Queries
{
  public class UserDto : ICustomMapping
  {
    public int Id { get; set; }
    public UserProfileDto Profile { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<BlockedUser, UserDto>()
        .ForMember(
          destination => destination.Id,
          options => options.MapFrom(x => x.BlockUserId))
        .ForMember(
          destination => destination.Profile,
          options => options.MapFrom(x => x.BlockUser.Profile));
    }
  }

  public class UserProfileDto : ICustomMapping
  {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<UserProfilePhotoDto> Photos { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<UserProfile, UserProfileDto>()
        .ForMember(
          destination => destination.Age,
          options => options.MapFrom(x => x.Birthday.GetAge()));
    }
  }

  public class UserProfilePhotoDto : ICustomMapping
  {
    public string Url { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<UserProfilePhoto, UserProfilePhotoDto>()
        .ForMember(
          destination => destination.Url,
          options => options.MapFrom(x => x.Attachment.Url));
    }
  }

  public class SelfUserDto
  {
    public int Id { get; set; }
    public SelfUserProfileDto Profile { get; set; }
  }

  public class SelfUserProfileDto
  {
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<UserProfilePhotoDto> Photos { get; set; }
  }
}

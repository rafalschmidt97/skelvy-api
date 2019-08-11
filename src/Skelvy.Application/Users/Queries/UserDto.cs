using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Mappers;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;
using AutoMapperProfile = AutoMapper.Profile;

namespace Skelvy.Application.Users.Queries
{
  public class UserDto : ICustomMapping
  {
    public int Id { get; set; }
    public ProfileDto Profile { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<Relation, UserDto>()
        .ForMember(
          destination => destination.Id,
          options => options.MapFrom(x => x.RelatedUserId))
        .ForMember(
          destination => destination.Profile,
          options => options.MapFrom(x => x.RelatedUser.Profile));
    }
  }

  public class ProfileDto : ICustomMapping
  {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<ProfilePhotoDto> Photos { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<Profile, ProfileDto>()
        .ForMember(
          destination => destination.Age,
          options => options.MapFrom(x => x.Birthday.GetAge()));
    }
  }

  public class ProfilePhotoDto : ICustomMapping
  {
    public string Url { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<ProfilePhoto, ProfilePhotoDto>()
        .ForMember(
          destination => destination.Url,
          options => options.MapFrom(x => x.Attachment.Url));
    }
  }

  public class SelfUserDto
  {
    public int Id { get; set; }
    public SelfProfileDto Profile { get; set; }
  }

  public class SelfProfileDto
  {
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<ProfilePhotoDto> Photos { get; set; }
  }

  public class UserWithRelationTypeDto
  {
    public int Id { get; set; }
    public ProfileDto Profile { get; set; }
    public string RelationType { get; set; }
  }
}

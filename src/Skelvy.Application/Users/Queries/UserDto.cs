using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Mappers;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;
using AutoMapperProfile = AutoMapper.Profile;

namespace Skelvy.Application.Users.Queries
{
  public class UserDto : IMapping<User>, IMapping<Relation>
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public ProfileDto Profile { get; set; }

    public void Mapping(AutoMapperProfile profile)
    {
      profile.CreateMap<User, UserDto>();
      profile.CreateMap<Relation, UserDto>()
        .ForMember(
          destination => destination.Id,
          options => options.MapFrom(x => x.RelatedUserId))
        .ForMember(
          destination => destination.Name,
          options => options.MapFrom(x => x.RelatedUser.Name))
        .ForMember(
          destination => destination.Profile,
          options => options.MapFrom(x => x.RelatedUser.Profile));
    }
  }

  public class ProfileDto : IMapping<Profile>
  {
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<ProfilePhotoDto> Photos { get; set; }

    public void Mapping(AutoMapperProfile profile)
    {
      profile.CreateMap<Profile, ProfileDto>()
        .ForMember(
          destination => destination.Age,
          options => options.MapFrom(x => x.Birthday.GetAge()));
    }
  }

  public class ProfilePhotoDto : IMapping<ProfilePhoto>
  {
    public string Url { get; set; }

    public void Mapping(AutoMapperProfile profile)
    {
      profile.CreateMap<ProfilePhoto, ProfilePhotoDto>()
        .ForMember(
          destination => destination.Url,
          options => options.MapFrom(x => x.Attachment.Url));
    }
  }

  public class SelfUserDto : IMapping<User>
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public SelfProfileDto Profile { get; set; }
  }

  public class SelfProfileDto : IMapping<Profile>
  {
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public IList<ProfilePhotoDto> Photos { get; set; }
  }
}

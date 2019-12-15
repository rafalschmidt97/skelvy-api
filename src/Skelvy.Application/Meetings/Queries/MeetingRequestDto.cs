using System;
using System.Collections.Generic;
using System.Linq;
using Skelvy.Application.Activities.Queries;
using Skelvy.Application.Core.Mappers;
using Skelvy.Application.Users.Queries;
using Skelvy.Domain.Entities;
using AutoMapperProfile = AutoMapper.Profile;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingRequestDto : ICustomMapping
  {
    public int Id { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public IList<ActivityDto> Activities { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<MeetingRequest, MeetingRequestDto>()
        .ForMember(
          destination => destination.Activities,
          options => options.MapFrom(x => x.Activities.Select(y => y.Activity)))
        .ForMember(
          destination => destination.City,
          options => options.Ignore());
    }
  }

  public class MeetingRequestWithUserDto : ICustomMapping
  {
    public int Id { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; }
    public string City { get; set; }
    public IList<ActivityDto> Activities { get; set; }
    public UserDto User { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<MeetingRequest, MeetingRequestWithUserDto>()
        .ForMember(
          destination => destination.Activities,
          options => options.MapFrom(x => x.Activities.Select(y => y.Activity)))
        .ForMember(
          destination => destination.City,
          options => options.Ignore());
    }
  }
}

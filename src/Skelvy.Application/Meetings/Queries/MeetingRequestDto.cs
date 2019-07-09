using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Skelvy.Application.Core.Mappers;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Application.Users.Queries;
using Skelvy.Domain.Entities;

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
    public string City { get; set; }
    public IList<DrinkTypeDto> DrinkTypes { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<MeetingRequest, MeetingRequestDto>()
        .ForMember(
          destination => destination.DrinkTypes,
          options => options.MapFrom(x => x.DrinkTypes.Select(y => y.DrinkType)))
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
    public string City { get; set; }
    public IList<DrinkTypeDto> DrinkTypes { get; set; }
    public UserDto User { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<MeetingRequest, MeetingRequestWithUserDto>()
        .ForMember(
          destination => destination.DrinkTypes,
          options => options.MapFrom(x => x.DrinkTypes.Select(y => y.DrinkType)))
        .ForMember(
          destination => destination.City,
          options => options.Ignore());
    }
  }
}

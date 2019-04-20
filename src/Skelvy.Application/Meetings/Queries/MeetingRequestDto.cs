using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Skelvy.Application.Core.Mappers;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingRequestDto : ICustomMapping
  {
    public MeetingRequestDto(
      int id,
      DateTimeOffset minDate,
      DateTimeOffset maxDate,
      int minAge,
      int maxAge,
      double latitude,
      double longitude,
      IList<DrinkDto> drinks)
    {
      Id = id;
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      Drinks = drinks;
    }

    public int Id { get; }
    public DateTimeOffset MinDate { get; }
    public DateTimeOffset MaxDate { get; }
    public int MinAge { get; }
    public int MaxAge { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public IList<DrinkDto> Drinks { get; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<MeetingRequest, MeetingRequestDto>()
        .ForMember(
          destination => destination.Drinks,
          options => options.MapFrom(x => x.Drinks.Select(y => y.Drink)));
    }
  }
}

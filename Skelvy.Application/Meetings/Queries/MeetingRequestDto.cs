using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Skelvy.Application.Core.Mapping;
using Skelvy.Application.Drinks.Queries;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingRequestDto : ICustomMapping
  {
    public int Id { get; set; }
    public DateTime MinDate { get; set; }
    public DateTime MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<DrinkDto> Drinks { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<MeetingRequest, MeetingRequestDto>()
        .ForMember(
          destination => destination.Drinks,
          options => options.MapFrom(x => x.Drinks.Select(y => y.Drink)));
    }
  }
}

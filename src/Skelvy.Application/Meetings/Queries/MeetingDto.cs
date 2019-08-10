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
  public class MeetingDto : ICustomMapping
  {
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string City { get; set; }
    public int GroupId { get; set; }
    public IList<UserDto> Users { get; set; }
    public ActivityDto Activity { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<Meeting, MeetingDto>()
        .ForMember(
          destination => destination.Users,
          options => options.MapFrom(x => x.Group.Users.Select(y => y.User)))
        .ForMember(
          destination => destination.City,
          options => options.Ignore());
    }
  }
}

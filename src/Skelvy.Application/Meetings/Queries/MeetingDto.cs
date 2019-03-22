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
  public class MeetingDto : ICustomMapping
  {
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<UserDto> Users { get; set; }
    public DrinkDto Drink { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<Meeting, MeetingDto>()
        .ForMember(
          destination => destination.Users,
          options => options.MapFrom(x => x.Users.Select(y => y.User)));
    }
  }

  public class MeetingChatMessageDto
  {
    public string Message { get; set; }
    public DateTimeOffset Date { get; set; }
    public int UserId { get; set; }
    public int MeetingId { get; set; }
  }
}

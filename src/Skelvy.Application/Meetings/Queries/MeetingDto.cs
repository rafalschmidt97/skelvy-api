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
    public MeetingDto(int id, DateTimeOffset date, double latitude, double longitude, IList<UserDto> users, DrinkDto drink)
    {
      Id = id;
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      Users = users;
      Drink = drink;
    }

    public MeetingDto()
    {
    }

    public int Id { get; }
    public DateTimeOffset Date { get; }
    public double Latitude { get; }
    public double Longitude { get; }
    public IList<UserDto> Users { get; }
    public DrinkDto Drink { get; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<Meeting, MeetingDto>()
        .ForMember(
          destination => destination.Users,
          options => options.MapFrom(x => x.Users.Where(y => !y.IsRemoved).Select(y => y.User)));
    }
  }

  public class MeetingChatMessageDto
  {
    public MeetingChatMessageDto(string message, DateTimeOffset date, int userId, int meetingId)
    {
      Message = message;
      Date = date;
      UserId = userId;
      MeetingId = meetingId;
    }

    public MeetingChatMessageDto()
    {
    }

    public string Message { get; }
    public DateTimeOffset Date { get; }
    public int UserId { get; }
    public int MeetingId { get; }
  }
}

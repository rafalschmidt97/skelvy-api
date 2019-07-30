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
    public string City { get; set; }
    public int GroupId { get; set; }
    public IList<UserDto> Users { get; set; }
    public DrinkTypeDto DrinkType { get; set; }

    public void CreateMappings(Profile configuration)
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

  public class MessageDto : ICustomMapping
  {
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTimeOffset Date { get; set; }
    public int UserId { get; set; }
    public string AttachmentUrl { get; set; }
    public int GroupId { get; set; }

    public void CreateMappings(Profile configuration)
    {
      configuration.CreateMap<Message, MessageDto>()
        .ForMember(
          destination => destination.AttachmentUrl,
          options => options.MapFrom(x => x.Attachment.Url));
    }
  }
}

using System;
using System.Collections.Generic;
using Skelvy.Application.Activities.Queries;
using Skelvy.Application.Core.Mappers;
using Skelvy.Application.Messages.Queries;
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
    public int Size { get; set; }
    public bool IsHidden { get; set; }
    public string City { get; set; }
    public int GroupId { get; set; }
    public ActivityDto Activity { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<Meeting, MeetingDto>()
        .ForMember(
          destination => destination.City,
          options => options.Ignore());
    }
  }

  public class MeetingWithUsersDto : ICustomMapping
  {
    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string City { get; set; }
    public int GroupId { get; set; }
    public ActivityDto Activity { get; set; }
    public IList<GroupUserDto> Users { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<Meeting, MeetingWithUsersDto>()
        .ForMember(
          destination => destination.Users,
          options => options.MapFrom(x => x.Group.Users))
        .ForMember(
          destination => destination.City,
          options => options.Ignore());
    }
  }

  public class GroupDto
  {
    public int Id { get; set; }
    public IList<GroupUserDto> Users { get; set; }
    public IList<MessageDto> Messages { get; set; }
  }

  public class GroupUserDto : ICustomMapping
  {
    public int Id { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }
    public ProfileDto Profile { get; set; }

    public void CreateMappings(AutoMapperProfile configuration)
    {
      configuration.CreateMap<GroupUser, GroupUserDto>()
        .ForMember(
          destination => destination.Id,
          options => options.MapFrom(x => x.UserId))
        .ForMember(
          destination => destination.Role,
          options => options.MapFrom(x => x.Role))
        .ForMember(
          destination => destination.Name,
          options => options.MapFrom(x => x.User.Name))
        .ForMember(
          destination => destination.Profile,
          options => options.MapFrom(x => x.User.Profile));
    }
  }
}

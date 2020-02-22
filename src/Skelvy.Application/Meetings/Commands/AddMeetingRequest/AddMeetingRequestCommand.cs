using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Commands.AddMeetingRequest
{
  public class AddMeetingRequestCommand : ICommandData<MeetingRequestDto>
  {
    public AddMeetingRequestCommand(int userId, DateTimeOffset minDate, DateTimeOffset maxDate, int minAge, int maxAge, double latitude, double longitude, string description, IList<AddMeetingRequestActivity> activities)
    {
      UserId = userId;
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      Description = description;
      Activities = activities;
    }

    [JsonConstructor]
    public AddMeetingRequestCommand()
    {
    }

    public int UserId { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; }
    public IList<AddMeetingRequestActivity> Activities { get; set; }
  }

  public class AddMeetingRequestActivity
  {
    public AddMeetingRequestActivity(int id)
    {
      Id = id;
    }

    [JsonConstructor]
    public AddMeetingRequestActivity()
    {
    }

    public int Id { get; set; }
  }
}

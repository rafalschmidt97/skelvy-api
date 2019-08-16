using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.UpdateMeetingRequest
{
  public class UpdateMeetingRequestCommand : ICommand
  {
    public UpdateMeetingRequestCommand(int userId, int requestId, DateTimeOffset minDate, DateTimeOffset maxDate, int minAge, int maxAge, double latitude, double longitude, string description, IList<UpdateMeetingRequestActivity> activities)
    {
      UserId = userId;
      RequestId = requestId;
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      Description = description;
      Activities = activities;
    }

    public int UserId { get; set; }
    public int RequestId { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; }
    public IList<UpdateMeetingRequestActivity> Activities { get; set; }
  }

  public class UpdateMeetingRequestActivity
  {
    public UpdateMeetingRequestActivity(int id)
    {
      Id = id;
    }

    public int Id { get; set; }
  }
}

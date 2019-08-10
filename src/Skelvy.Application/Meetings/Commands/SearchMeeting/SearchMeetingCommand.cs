using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.SearchMeeting
{
  public class SearchMeetingCommand : ICommand
  {
    public SearchMeetingCommand(
      int userId,
      DateTimeOffset minDate,
      DateTimeOffset maxDate,
      int minAge,
      int maxAge,
      double latitude,
      double longitude,
      IList<CreateMeetingRequestActivity> activities)
    {
      UserId = userId;
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      Activities = activities;
    }

    public int UserId { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public IList<CreateMeetingRequestActivity> Activities { get; set; }
  }

  public class CreateMeetingRequestActivity
  {
    public CreateMeetingRequestActivity(int id)
    {
      Id = id;
    }

    public int Id { get; set; }
  }
}

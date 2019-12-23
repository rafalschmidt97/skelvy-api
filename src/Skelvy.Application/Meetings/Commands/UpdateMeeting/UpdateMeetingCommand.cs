using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.UpdateMeeting
{
  public class UpdateMeetingCommand : ICommand
  {
    public UpdateMeetingCommand(int userId, int meetingId, DateTimeOffset date, double latitude, double longitude, int size, string description, int activityId, bool isHidden)
    {
      UserId = userId;
      MeetingId = meetingId;
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      Size = size;
      Description = description;
      ActivityId = activityId;
      IsHidden = isHidden;
    }

    public int UserId { get; set; }
    public int MeetingId { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Size { get; set; }
    public string Description { get; set; }
    public int ActivityId { get; set; }
    public bool IsHidden { get; set; }
  }
}

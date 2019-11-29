using System;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Commands.AddMeeting
{
  public class AddMeetingCommand : ICommandData<MeetingDto>
  {
    public AddMeetingCommand(int userId, DateTimeOffset date, double latitude, double longitude, int size, int activityId, bool isHidden)
    {
      UserId = userId;
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      Size = size;
      ActivityId = activityId;
      IsHidden = isHidden;
    }

    public int UserId { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Size { get; set; }
    public int ActivityId { get; set; }
    public bool IsHidden { get; set; }
  }
}

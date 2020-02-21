using System;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Meetings.Commands.AddMeeting
{
  public class AddMeetingCommand : ICommandData<MeetingDto>
  {
    public AddMeetingCommand(int userId, DateTimeOffset date, double latitude, double longitude, int size, string description, int activityId, bool isHidden)
    {
      UserId = userId;
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      Size = size;
      Description = description;
      ActivityId = activityId;
      IsHidden = isHidden;
    }

    [JsonConstructor]
    public AddMeetingCommand()
    {
    }

    public int UserId { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Size { get; set; }
    public string Description { get; set; }
    public int ActivityId { get; set; }
    public bool IsHidden { get; set; }
  }
}

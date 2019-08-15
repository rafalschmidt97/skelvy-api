using System;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.ConnectMeetingRequest
{
  public class ConnectMeetingRequestCommand : ICommand
  {
    public ConnectMeetingRequestCommand(int userId, int meetingRequestId, DateTimeOffset date, int activityId)
    {
      UserId = userId;
      MeetingRequestId = meetingRequestId;
      Date = date;
      ActivityId = activityId;
    }

    public int UserId { get; set; }
    public int MeetingRequestId { get; set; }
    public DateTimeOffset Date { get; set; }
    public int ActivityId { get; set; }
  }
}

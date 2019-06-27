using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.ConnectMeetingRequest
{
  public class ConnectMeetingRequestCommand : ICommand
  {
    public ConnectMeetingRequestCommand(int userId, int meetingRequestId)
    {
      UserId = userId;
      MeetingRequestId = meetingRequestId;
    }

    public int UserId { get; set; }
    public int MeetingRequestId { get; set; }
  }
}

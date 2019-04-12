using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQuery : IQuery<MeetingViewModel>
  {
    public int UserId { get; set; }
  }
}

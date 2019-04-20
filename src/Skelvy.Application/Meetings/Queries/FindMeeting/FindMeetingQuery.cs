using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQuery : IQuery<MeetingViewModel>
  {
    public FindMeetingQuery(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}

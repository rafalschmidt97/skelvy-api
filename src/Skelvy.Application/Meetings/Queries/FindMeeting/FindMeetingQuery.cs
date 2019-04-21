using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQuery : IQuery<MeetingModel>
  {
    public FindMeetingQuery(int userId)
    {
      UserId = userId;
    }

    public int UserId { get; set; }
  }
}

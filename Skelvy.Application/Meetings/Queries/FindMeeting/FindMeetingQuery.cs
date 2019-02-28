using MediatR;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQuery : IRequest<MeetingViewModel>
  {
    public int UserId { get; set; }
  }
}

using MediatR;

namespace Skelvy.Application.Meetings.Queries.GetMeeting
{
  public class GetMeetingQuery : IRequest<MeetingViewModel>
  {
    public int UserId { get; set; }
  }
}

using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQuery : IQuery<MeetingDto>
  {
    public FindMeetingQuery(int meetingId, int userId, string language)
    {
      MeetingId = meetingId;
      UserId = userId;
      Language = language;
    }

    public FindMeetingQuery() // required for FromQuery attribute
    {
    }

    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

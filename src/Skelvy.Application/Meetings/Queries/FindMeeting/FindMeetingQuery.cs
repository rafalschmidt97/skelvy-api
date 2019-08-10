using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Meetings.Queries.FindMeeting
{
  public class FindMeetingQuery : IQuery<MeetingModel>
  {
    public FindMeetingQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    public FindMeetingQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeetings
{
  public class FindMeetingsQuery : IQuery<MeetingModel>
  {
    public FindMeetingsQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    [JsonConstructor]
    public FindMeetingsQuery()
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

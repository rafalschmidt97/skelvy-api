using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeetingRequests
{
  public class FindMeetingRequestsQuery : IQuery<IList<MeetingRequestDto>>
  {
    public FindMeetingRequestsQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    [JsonConstructor]
    public FindMeetingRequestsQuery()
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

using System.Collections.Generic;
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

    public FindMeetingRequestsQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

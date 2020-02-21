using System.Collections.Generic;
using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Queries.FindMeetingInvitations
{
  public class FindMeetingInvitationsQuery : IQuery<IList<SelfMeetingInvitationDto>>
  {
    public FindMeetingInvitationsQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    [JsonConstructor]
    public FindMeetingInvitationsQuery()
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

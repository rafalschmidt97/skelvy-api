using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Groups.Queries.FindGroup
{
  public class FindGroupQuery : IQuery<GroupDto>
  {
    public FindGroupQuery(int groupId, int userId, string language)
    {
      GroupId = groupId;
      UserId = userId;
      Language = language;
    }

    public FindGroupQuery() // required for FromQuery attribute
    {
    }

    public int GroupId { get; set; }
    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

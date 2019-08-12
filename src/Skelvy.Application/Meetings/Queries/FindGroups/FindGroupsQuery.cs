using Skelvy.Application.Core.Bus;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Meetings.Queries.FindGroups
{
  public class FindGroupsQuery : IQuery<GroupsModel>
  {
    public FindGroupsQuery(int userId, string language)
    {
      UserId = userId;
      Language = language;
    }

    public FindGroupsQuery() // required for FromQuery attribute
    {
    }

    public int UserId { get; set; }
    public string Language { get; set; } = LanguageType.EN;
  }
}

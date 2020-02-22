using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Groups.Queries.FindGroup
{
  public class FindGroupQuery : IQuery<GroupDto>
  {
    public FindGroupQuery(int groupId, int userId)
    {
      GroupId = groupId;
      UserId = userId;
    }

    [JsonConstructor]
    public FindGroupQuery()
    {
    }

    public int GroupId { get; set; }
    public int UserId { get; set; }
  }
}

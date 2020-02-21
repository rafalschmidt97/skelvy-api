using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Relations.Queries.FindRelation
{
  public class FindRelationQuery : IQuery<RelationDto>
  {
    public FindRelationQuery(int userId, int relatedUserId)
    {
      UserId = userId;
      RelatedUserId = relatedUserId;
    }

    [JsonConstructor]
    public FindRelationQuery()
    {
    }

    public int UserId { get; set; }
    public int RelatedUserId { get; set; }
  }
}

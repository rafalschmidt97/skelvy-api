using Skelvy.Application.Core.Mappers;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Queries
{
  public class RelationDto : IMapping<Relation>
  {
    public int UserId { get; set; }
    public int RelatedUserId { get; set; }
    public string Type { get; set; }
  }
}

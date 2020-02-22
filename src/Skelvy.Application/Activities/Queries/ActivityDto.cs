using Skelvy.Application.Core.Mappers;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Activities.Queries
{
  public class ActivityDto : IMapping<Activity>
  {
    public int Id { get; set; }
    public string Type { get; set; }
    public string Name { get; set; }
    public int Size { get; set; }
  }
}

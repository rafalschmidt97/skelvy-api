using System;
using Skelvy.Domain.Entities.Core;

namespace Skelvy.Domain.Entities
{
  public class Attachment : ICreatableEntity
  {
    public Attachment(string type, string url)
    {
      Type = type;
      Url = url;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

using System;
using Skelvy.Domain.Entities.Base;

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

    public Attachment(int id, string type, string url, DateTimeOffset createdAt)
    {
      Id = id;
      Type = type;
      Url = url;
      CreatedAt = createdAt;
    }

    public int Id { get; private set; }
    public string Type { get; private set; }
    public string Url { get; private set; }
    public DateTimeOffset CreatedAt { get; }
  }
}

using System;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Relation : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public Relation(int userId, int relatedUserId, string type)
    {
      UserId = userId;
      RelatedUserId = relatedUserId;
      Type = type;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
    public int RelatedUserId { get; set; }
    public string Type { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }

    public User User { get; set; }
    public User RelatedUser { get; set; }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(Relation)}(Id = {Id}) is already aborted.");
      }
    }
  }
}

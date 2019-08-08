using System;
using Skelvy.Domain.Entities.Base;
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

    public Relation(int id, int userId, int relatedUserId, string type, DateTimeOffset createdAt, DateTimeOffset? modifiedAt, bool isRemoved)
    {
      Id = id;
      UserId = userId;
      RelatedUserId = relatedUserId;
      Type = type;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
    }

    public int Id { get; set; }
    public int UserId { get; private set; }
    public int RelatedUserId { get; private set; }
    public string Type { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }

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

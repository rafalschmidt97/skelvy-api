using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class BlockedUser : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public BlockedUser(int userId, int blockUserId)
    {
      UserId = userId;
      BlockUserId = blockUserId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public BlockedUser(
      int id,
      DateTimeOffset createdAt,
      DateTimeOffset? modifiedAt,
      bool isRemoved,
      int userId,
      int blockUserId,
      User user,
      User blockUser)
    {
      Id = id;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
      UserId = userId;
      BlockUserId = blockUserId;
      User = user;
      BlockUser = blockUser;
    }

    public int Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public int UserId { get; private set; }
    public int BlockUserId { get; private set; }

    public User User { get; private set; }
    public User BlockUser { get; private set; }

    public void Remove()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(BlockedUser)}(UserId = {UserId}, BlockedUserId = {BlockUserId}) is already removed.");
      }
    }
  }
}

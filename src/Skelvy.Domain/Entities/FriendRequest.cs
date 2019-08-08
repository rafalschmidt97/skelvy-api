using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Users;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class FriendRequest : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public FriendRequest(int invitingUserId, int invitedUserId)
    {
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; private set; }
    public int InvitingUserId { get; private set; }
    public int InvitedUserId { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }

    public User InvitingUser { get; set; }
    public User InvitedUser { get; set; }

    public void Accept()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        Status = FriendRequestStatusTypes.Accepted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(FriendRequest)}(Id = {Id}) is already accepted.");
      }
    }

    public void Deny()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        Status = FriendRequestStatusTypes.Denied;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(FriendRequest)}(Id = {Id}) is already denied.");
      }
    }
  }
}

using System;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class FriendRequest : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public FriendRequest(int invitingUserId, int invitedUserId)
    {
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;

      Status = FriendRequestStatusType.Pending;
      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public int InvitingUserId { get; set; }
    public int InvitedUserId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }

    public User InvitingUser { get; set; }
    public User InvitedUser { get; set; }

    public void Accept()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        Status = FriendRequestStatusType.Accepted;
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
        Status = FriendRequestStatusType.Denied;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(FriendRequest)}(Id = {Id}) is already denied.");
      }
    }
  }
}

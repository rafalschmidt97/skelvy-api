using System;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class MeetingInvitation : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public MeetingInvitation(int invitingUserId, int invitedUserId, int meetingId)
    {
      InvitingUserId = invitingUserId;
      InvitedUserId = invitedUserId;
      MeetingId = meetingId;

      Status = MeetingInvitationStatusType.Pending;
      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public int InvitingUserId { get; set; }
    public int InvitedUserId { get; set; }
    public int MeetingId { get; set; }
    public string Status { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }

    public User InvitingUser { get; set; }
    public User InvitedUser { get; set; }
    public Meeting Meeting { get; set; }

    public void Accept()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        Status = MeetingInvitationStatusType.Accepted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(MeetingInvitation)}({Id}) is already accepted.");
      }
    }

    public void Deny()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        Status = MeetingInvitationStatusType.Denied;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(MeetingInvitation)}({Id}) is already denied.");
      }
    }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        Status = MeetingInvitationStatusType.Aborted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(MeetingInvitation)}({Id}) is already aborted.");
      }
    }
  }
}

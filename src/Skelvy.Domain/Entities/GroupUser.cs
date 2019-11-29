using System;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class GroupUser : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public GroupUser(int groupId, int userId, int meetingRequestId, string role = GroupUserRoleType.Member)
    {
      GroupId = groupId;
      UserId = userId;
      MeetingRequestId = meetingRequestId;
      Role = role;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public GroupUser(int groupId, int userId, string role = GroupUserRoleType.Member)
    {
      GroupId = groupId;
      UserId = userId;
      Role = role;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public int GroupId { get; set; }
    public int UserId { get; set; }
    public int? MeetingRequestId { get; set; }
    public string Role { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public string RemovedReason { get; set; }

    public Group Group { get; set; }
    public User User { get; set; }
    public MeetingRequest MeetingRequest { get; set; }

    public bool CanAddUserToGroup => Role != GroupUserRoleType.Owner ||
                                     Role != GroupUserRoleType.Admin ||
                                     Role != GroupUserRoleType.Privileged;

    public bool CanRemoveUserFromGroup(GroupUser groupUser)
    {
      return (Role == GroupUserRoleType.Owner || Role == GroupUserRoleType.Admin) &&
             groupUser.Role != GroupUserRoleType.Owner;
    }

    public bool CanUpdateRole(GroupUser groupUser, string role)
    {
      return (Role == GroupUserRoleType.Owner ||
             (Role == GroupUserRoleType.Admin && role != GroupUserRoleType.Owner)) &&
             groupUser.Role != GroupUserRoleType.Owner;
    }

    public void UpdateRole(string role)
    {
      Role = role == GroupUserRoleType.Admin || role == GroupUserRoleType.Privileged || role == GroupUserRoleType.Member
        ? role
        : throw new DomainException(
          $"'Role' must be {GroupUserRoleType.Admin} / {GroupUserRoleType.Privileged} / {GroupUserRoleType.Member} for {nameof(GroupUser)}({Id}).");

      ModifiedAt = DateTimeOffset.UtcNow;
    }

    public void Leave()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = GroupUserRemovedReasonType.Left;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(GroupUser)}({Id}) is already left.");
      }
    }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = GroupUserRemovedReasonType.Aborted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(GroupUser)}({Id}) is already aborted.");
      }
    }

    public void Remove()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = GroupUserRemovedReasonType.Removed;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(GroupUser)}({Id}) is already removed.");
      }
    }
  }
}

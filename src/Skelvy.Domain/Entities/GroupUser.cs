using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class GroupUser : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public GroupUser(int groupId, int userId, int meetingRequestId)
    {
      GroupId = groupId;
      UserId = userId;
      MeetingRequestId = meetingRequestId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public int GroupId { get; set; }
    public int UserId { get; set; }
    public int MeetingRequestId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public string RemovedReason { get; set; }

    public Group Group { get; set; }
    public User User { get; set; }
    public MeetingRequest MeetingRequest { get; set; }

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
        throw new DomainException($"Entity {nameof(GroupUser)}(Id = {Id}) is already left.");
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
        throw new DomainException($"Entity {nameof(GroupUser)}(Id = {Id}) is already left.");
      }
    }
  }
}

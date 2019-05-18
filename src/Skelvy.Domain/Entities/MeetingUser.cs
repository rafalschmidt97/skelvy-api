using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class MeetingUser : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public MeetingUser(int meetingId, int userId, int meetingRequestId)
    {
      MeetingId = meetingId;
      UserId = userId;
      MeetingRequestId = meetingRequestId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public MeetingUser(
      int id,
      DateTimeOffset createdAt,
      DateTimeOffset? modifiedAt,
      bool isRemoved,
      string removedReason,
      int meetingId,
      int userId,
      int meetingRequestId,
      Meeting meeting,
      User user,
      MeetingRequest meetingRequest)
    {
      Id = id;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
      RemovedReason = removedReason;
      MeetingId = meetingId;
      UserId = userId;
      MeetingRequestId = meetingRequestId;
      Meeting = meeting;
      User = user;
      MeetingRequest = meetingRequest;
    }

    public int Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public string RemovedReason { get; private set; }
    public int MeetingId { get; private set; }
    public int UserId { get; private set; }
    public int MeetingRequestId { get; private set; }

    public Meeting Meeting { get; private set; }
    public User User { get; private set; }
    public MeetingRequest MeetingRequest { get; private set; }

    public void Leave()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingUserRemovedReasonTypes.Left;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingUser)}(Id = {Id}) is already left.");
      }
    }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingUserRemovedReasonTypes.Aborted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingUser)}(Id = {Id}) is already left.");
      }
    }
  }
}

using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Domain.Entities
{
  public class MeetingUser : ICreatableEntity, IRemovableEntity
  {
    public MeetingUser(int meetingId, int userId, int meetingRequestId)
    {
      MeetingId = meetingId;
      UserId = userId;
      MeetingRequestId = meetingRequestId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public MeetingUser(int id, int meetingId, int userId, int meetingRequestId)
      : this(meetingId, userId, meetingRequestId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedAt { get; private set; }
    public string RemovedReason { get; private set; }
    public int MeetingId { get; private set; }
    public int UserId { get; private set; }
    public int MeetingRequestId { get; private set; }

    public Meeting Meeting { get; private set; }
    public User User { get; private set; }
    public MeetingRequest MeetingRequest { get; private set; }

    public void Abort()
    {
      IsRemoved = true;
      RemovedAt = DateTimeOffset.UtcNow;
      RemovedReason = MeetingUserRemovedReasonTypes.Aborted;
    }

    public void Expire()
    {
      IsRemoved = true;
      RemovedAt = DateTimeOffset.UtcNow;
      RemovedReason = MeetingRequestRemovedReasonTypes.Expired;
    }
  }
}

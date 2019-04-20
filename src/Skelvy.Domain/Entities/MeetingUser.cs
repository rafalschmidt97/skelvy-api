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

      CreatedDate = DateTimeOffset.UtcNow;
    }

    public MeetingUser(int id, int meetingId, int userId, int meetingRequestId)
      : this(meetingId, userId, meetingRequestId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedDate { get; private set; }
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
      RemovedDate = DateTimeOffset.UtcNow;
      RemovedReason = MeetingUserRemovedReasonTypes.Aborted;
    }

    public void Expire()
    {
      IsRemoved = true;
      RemovedDate = DateTimeOffset.UtcNow;
      RemovedReason = MeetingRequestRemovedReasonTypes.Expired;
    }
  }
}

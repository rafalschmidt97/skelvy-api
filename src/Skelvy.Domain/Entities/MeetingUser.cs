using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Exceptions;

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

    public MeetingUser(
      int id,
      DateTimeOffset createdAt,
      bool isRemoved,
      DateTimeOffset? removedAt,
      int meetingId,
      int userId,
      int meetingRequestId,
      Meeting meeting,
      User user,
      MeetingRequest meetingRequest)
    {
      Id = id;
      CreatedAt = createdAt;
      IsRemoved = isRemoved;
      RemovedAt = removedAt;
      MeetingId = meetingId;
      UserId = userId;
      MeetingRequestId = meetingRequestId;
      Meeting = meeting;
      User = user;
      MeetingRequest = meetingRequest;
    }

    public int Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedAt { get; private set; }
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
        RemovedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingUser)}(Id = {Id}) is already left.");
      }
    }
  }
}

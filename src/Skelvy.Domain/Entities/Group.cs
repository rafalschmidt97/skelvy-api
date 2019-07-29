using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Group : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public Group()
    {
      CreatedAt = DateTimeOffset.UtcNow;
      Users = new List<GroupUser>();
      ChatMessages = new List<MeetingChatMessage>();
    }

    public Group(int id, DateTimeOffset createdAt, DateTimeOffset? modifiedAt, bool isRemoved, string removedReason, IList<GroupUser> users, IList<MeetingChatMessage> chatMessages)
    {
      Id = id;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
      RemovedReason = removedReason;
      Users = users;
      ChatMessages = chatMessages;
    }

    public int Id { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public string RemovedReason { get; private set; }

    public IList<GroupUser> Users { get; set; }
    public IList<MeetingChatMessage> ChatMessages { get; private set; }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingRemovedReasonTypes.Aborted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(Meeting)}(Id = {Id}) is already aborted.");
      }
    }

    public void Expire()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingRemovedReasonTypes.Expired;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(Meeting)}(Id = {Id}) is already expired.");
      }
    }
  }
}

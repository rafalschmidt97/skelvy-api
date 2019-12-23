using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Group : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public Group()
    {
      CreatedAt = DateTimeOffset.UtcNow;
    }

    public Group(string name)
    {
      Name = name;
      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public string RemovedReason { get; set; }

    public IList<GroupUser> Users { get; set; }
    public IList<Message> Messages { get; set; }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingRemovedReasonType.Aborted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(Meeting)}({Id}) is already aborted.");
      }
    }

    public void Expire()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingRemovedReasonType.Expired;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(Meeting)}({Id}) is already expired.");
      }
    }
  }
}

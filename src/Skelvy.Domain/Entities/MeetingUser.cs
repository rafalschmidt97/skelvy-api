using System;
using Skelvy.Domain.Entities.Base;

namespace Skelvy.Domain.Entities
{
  public class MeetingUser : ICreatableEntity, IRemovableEntity
  {
    public MeetingUser()
    {
      CreatedDate = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? RemovedDate { get; set; }
    public string RemovedReason { get; set; }
    public int MeetingId { get; set; }
    public int UserId { get; set; }
    public int MeetingRequestId { get; set; }

    public Meeting Meeting { get; set; }
    public User User { get; set; }
    public MeetingRequest MeetingRequest { get; set; }

    public void Remove(string reason)
    {
      IsRemoved = true;
      RemovedDate = DateTimeOffset.UtcNow;
      RemovedReason = reason;
    }
  }
}

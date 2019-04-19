using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;

namespace Skelvy.Domain.Entities
{
  public class Meeting : ICreatableEntity, IRemovableEntity
  {
    public Meeting()
    {
      CreatedDate = DateTimeOffset.UtcNow;
      Users = new List<MeetingUser>();
      ChatMessages = new List<MeetingChatMessage>();
    }

    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? RemovedDate { get; set; }
    public string RemovedReason { get; set; }
    public int DrinkId { get; set; }

    public IList<MeetingUser> Users { get; private set; }
    public IList<MeetingChatMessage> ChatMessages { get; private set; }
    public Drink Drink { get; set; }

    public void Remove(string reason)
    {
      IsRemoved = true;
      RemovedDate = DateTimeOffset.UtcNow;
      RemovedReason = reason;
    }
  }
}

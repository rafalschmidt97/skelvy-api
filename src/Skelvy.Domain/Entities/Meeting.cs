using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Domain.Entities
{
  public class Meeting : ICreatableEntity, IRemovableEntity
  {
    public Meeting(DateTimeOffset date, double latitude, double longitude, int drinkId)
    {
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      DrinkId = drinkId;

      CreatedAt = DateTimeOffset.UtcNow;
      Users = new List<MeetingUser>();
      ChatMessages = new List<MeetingChatMessage>();
    }

    public Meeting(int id, DateTimeOffset date, double latitude, double longitude, int drinkId)
      : this(date, latitude, longitude, drinkId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedAt { get; private set; }
    public string RemovedReason { get; private set; }
    public int DrinkId { get; private set; }

    public IList<MeetingUser> Users { get; private set; }
    public IList<MeetingChatMessage> ChatMessages { get; private set; }
    public Drink Drink { get; private set; }

    public void Abort()
    {
      IsRemoved = true;
      RemovedAt = DateTimeOffset.UtcNow;
      RemovedReason = MeetingRemovedReasonTypes.Aborted;
    }

    public void Expire()
    {
      IsRemoved = true;
      RemovedAt = DateTimeOffset.UtcNow;
      RemovedReason = MeetingRemovedReasonTypes.Expired;
    }
  }
}

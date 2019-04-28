using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

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

    public Meeting(
      int id,
      DateTimeOffset date,
      double latitude,
      double longitude,
      DateTimeOffset createdAt,
      bool isRemoved,
      DateTimeOffset? removedAt,
      string removedReason,
      int drinkId,
      IList<MeetingUser> users,
      IList<MeetingChatMessage> chatMessages,
      Drink drink)
    {
      Id = id;
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      CreatedAt = createdAt;
      IsRemoved = isRemoved;
      RemovedAt = removedAt;
      RemovedReason = removedReason;
      DrinkId = drinkId;
      Users = users;
      ChatMessages = chatMessages;
      Drink = drink;
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
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedAt = DateTimeOffset.UtcNow;
        RemovedReason = MeetingRemovedReasonTypes.Aborted;
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
        RemovedAt = DateTimeOffset.UtcNow;
        RemovedReason = MeetingRemovedReasonTypes.Expired;
      }
      else
      {
        throw new DomainException($"Entity {nameof(Meeting)}(Id = {Id}) is already expired.");
      }
    }
  }
}

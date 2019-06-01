using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Meeting : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public Meeting(DateTimeOffset date, double latitude, double longitude, int drinkTypeId)
    {
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      DrinkTypeId = drinkTypeId;

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
      DateTimeOffset? modifiedAt,
      bool isRemoved,
      string removedReason,
      int drinkTypeId,
      IList<MeetingUser> users,
      IList<MeetingChatMessage> chatMessages,
      DrinkType drinkType)
    {
      Id = id;
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
      RemovedReason = removedReason;
      DrinkTypeId = drinkTypeId;
      Users = users;
      ChatMessages = chatMessages;
      DrinkType = drinkType;
    }

    public int Id { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public string RemovedReason { get; private set; }
    public int DrinkTypeId { get; private set; }

    public IList<MeetingUser> Users { get; set; }
    public IList<MeetingChatMessage> ChatMessages { get; private set; }
    public DrinkType DrinkType { get; private set; }

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

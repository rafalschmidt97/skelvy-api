using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Meeting : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public Meeting(DateTimeOffset date, double latitude, double longitude, int groupId, int drinkTypeId)
    {
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      GroupId = groupId;
      DrinkTypeId = drinkTypeId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public Meeting(int id, DateTimeOffset date, double latitude, double longitude, int groupId, int drinkTypeId, DateTimeOffset createdAt, DateTimeOffset? modifiedAt, bool isRemoved, string removedReason, Group group, DrinkType drinkType)
    {
      Id = id;
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      GroupId = groupId;
      DrinkTypeId = drinkTypeId;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
      RemovedReason = removedReason;
      Group = group;
      DrinkType = drinkType;
    }

    public int Id { get; private set; }
    public DateTimeOffset Date { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public int GroupId { get; private set; }
    public int DrinkTypeId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public string RemovedReason { get; private set; }

    public Group Group { get; private set; }
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

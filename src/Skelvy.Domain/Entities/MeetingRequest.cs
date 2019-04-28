using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class MeetingRequest : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public MeetingRequest(
      DateTimeOffset minDate,
      DateTimeOffset maxDate,
      int minAge,
      int maxAge,
      double latitude,
      double longitude,
      int userId)
    {
      Status = MeetingRequestStatusTypes.Searching;
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      UserId = userId;

      CreatedAt = DateTimeOffset.UtcNow;
      Drinks = new List<MeetingRequestDrink>();
    }

    public MeetingRequest(
      int id,
      string status,
      DateTimeOffset minDate,
      DateTimeOffset maxDate,
      int minAge,
      int maxAge,
      double latitude,
      double longitude,
      DateTimeOffset createdAt,
      DateTimeOffset? modifiedAt,
      bool isRemoved,
      DateTimeOffset? removedAt,
      string removedReason,
      int userId,
      IList<MeetingRequestDrink> drinks,
      User user)
    {
      Id = id;
      Status = status;
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      CreatedAt = createdAt;
      ModifiedAt = modifiedAt;
      IsRemoved = isRemoved;
      RemovedAt = removedAt;
      RemovedReason = removedReason;
      UserId = userId;
      Drinks = drinks;
      User = user;
    }

    public int Id { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset MinDate { get; private set; }
    public DateTimeOffset MaxDate { get; private set; }
    public int MinAge { get; private set; }
    public int MaxAge { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? ModifiedAt { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedAt { get; private set; }
    public string RemovedReason { get; private set; }
    public int UserId { get; private set; }

    public IList<MeetingRequestDrink> Drinks { get; private set; }
    public User User { get; private set; }

    public bool IsSearching => Status == MeetingRequestStatusTypes.Searching;
    public bool IsFound => Status == MeetingRequestStatusTypes.Found;

    public void MarkAsSearching()
    {
      if (!IsSearching)
      {
        Status = MeetingRequestStatusTypes.Searching;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already marked as searching.");
      }
    }

    public void MarkAsFound()
    {
      if (!IsFound)
      {
        Status = MeetingRequestStatusTypes.Found;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already marked as found.");
      }
    }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedAt = DateTimeOffset.UtcNow;
        RemovedReason = MeetingRequestRemovedReasonTypes.Aborted;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already aborted.");
      }
    }

    public void Expire()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedAt = DateTimeOffset.UtcNow;
        RemovedReason = MeetingRequestRemovedReasonTypes.Expired;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already expired.");
      }
    }
  }
}

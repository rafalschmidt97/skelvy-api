using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class MeetingRequest : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public MeetingRequest(DateTimeOffset minDate, DateTimeOffset maxDate, int minAge, int maxAge, double latitude, double longitude, int userId)
    {
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      UserId = userId;

      Status = MeetingRequestStatusTypes.Searching;
      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public string Status { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public string RemovedReason { get; set; }

    public IList<MeetingRequestDrinkType> DrinkTypes { get; set; }
    public User User { get; set; }

    public bool IsSearching => Status == MeetingRequestStatusTypes.Searching;
    public bool IsFound => Status == MeetingRequestStatusTypes.Found;

    public void MarkAsSearching()
    {
      if (!IsRemoved)
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
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already removed.");
      }
    }

    public void MarkAsFound()
    {
      if (!IsRemoved)
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
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already removed.");
      }
    }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingRequestRemovedReasonTypes.Aborted;
        ModifiedAt = DateTimeOffset.UtcNow;
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
        RemovedReason = MeetingRequestRemovedReasonTypes.Expired;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already expired.");
      }
    }
  }
}

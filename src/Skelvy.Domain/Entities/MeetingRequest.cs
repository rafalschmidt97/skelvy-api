using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class MeetingRequest : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public MeetingRequest(DateTimeOffset minDate, DateTimeOffset maxDate, int minAge, int maxAge, double latitude, double longitude, string description, int userId)
    {
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      Description = description;
      UserId = userId;

      Status = MeetingRequestStatusType.Searching;
      CreatedAt = DateTimeOffset.UtcNow;
    }

    public MeetingRequest(DateTimeOffset minDate, DateTimeOffset maxDate, int minAge, int maxAge, double latitude, double longitude, int userId)
      : this(minDate, maxDate, minAge, maxAge, latitude, longitude, null, userId)
    {
    }

    public int Id { get; set; }
    public string Status { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public string RemovedReason { get; set; }

    public IList<MeetingRequestActivity> Activities { get; set; }
    public User User { get; set; }

    public bool IsSearching => Status == MeetingRequestStatusType.Searching;
    public bool IsFound => Status == MeetingRequestStatusType.Found;

    public void MarkAsSearching()
    {
      if (!IsRemoved)
      {
        if (!IsSearching)
        {
          Status = MeetingRequestStatusType.Searching;
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
          Status = MeetingRequestStatusType.Found;
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
        RemovedReason = MeetingRequestRemovedReasonType.Aborted;
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
        RemovedReason = MeetingRequestRemovedReasonType.Expired;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"Entity {nameof(MeetingRequest)}(Id = {Id}) is already expired.");
      }
    }
  }
}

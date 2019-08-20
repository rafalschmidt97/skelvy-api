using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
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
      Description = description?.Trim();
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

    public void Update(DateTimeOffset minDate, DateTimeOffset maxDate, int minAge, int maxAge, double latitude, double longitude, string description)
    {
      MinDate = minDate >= DateTimeOffset.UtcNow.AddDays(-1)
        ? minDate
        : throw new DomainException(
          $"'MinDate' must show the future for {nameof(MeetingRequest)}({Id}).");

      MaxDate = maxDate >= minDate
        ? maxDate
        : throw new DomainException(
          $"'MaxDate' must be after 'MinDate' for {nameof(MeetingRequest)}({Id}).");

      MinAge = minAge >= 18
        ? minAge
        : throw new DomainException(
          $"'MinAge' must show the age of majority for {nameof(MeetingRequest)}({Id}).");

      MaxAge = maxAge >= minAge && maxAge - minAge >= 5 && maxAge <= 55
        ? maxAge
        : throw new DomainException(
          "'MaxAge' must be bigger than 'MinAge' and age difference must be more or equal to 5 years and " +
          $"'MaxAge' must be less or equal 55 for  {nameof(MeetingRequest)}({Id}).");

      Latitude = latitude;
      Longitude = longitude;
      Description = description?.Trim();

      ModifiedAt = DateTimeOffset.UtcNow;
    }

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
          throw new DomainException($"{nameof(MeetingRequest)}({Id}) is already marked as searching.");
        }
      }
      else
      {
        throw new DomainException($"{nameof(MeetingRequest)}({Id}) is already removed.");
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
          throw new DomainException($"{nameof(MeetingRequest)}({Id}) is already marked as found.");
        }
      }
      else
      {
        throw new DomainException($"{nameof(MeetingRequest)}({Id}) is already removed.");
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
        throw new DomainException($"{nameof(MeetingRequest)}({Id}) is already aborted.");
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
        throw new DomainException($"{nameof(MeetingRequest)}({Id}) is already expired.");
      }
    }
  }
}

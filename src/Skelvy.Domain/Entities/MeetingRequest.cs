using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;

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

      CreatedDate = DateTimeOffset.UtcNow;
      Drinks = new List<MeetingRequestDrink>();
    }

    public MeetingRequest(
      int id,
      DateTimeOffset minDate,
      DateTimeOffset maxDate,
      int minAge,
      int maxAge,
      double latitude,
      double longitude,
      int userId)
      : this(minDate, maxDate, minAge, maxAge, latitude, longitude, userId)
    {
      Id = id;
    }

    public int Id { get; private set; }
    public string Status { get; private set; }
    public DateTimeOffset MinDate { get; private set; }
    public DateTimeOffset MaxDate { get; private set; }
    public int MinAge { get; private set; }
    public int MaxAge { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public DateTimeOffset CreatedDate { get; private set; }
    public DateTimeOffset? ModifiedDate { get; private set; }
    public bool IsRemoved { get; private set; }
    public DateTimeOffset? RemovedDate { get; private set; }
    public string RemovedReason { get; private set; }
    public int UserId { get; private set; }

    public IList<MeetingRequestDrink> Drinks { get; private set; }
    public User User { get; private set; }

    public bool IsSearching => Status == MeetingRequestStatusTypes.Searching;
    public bool IsFound => Status == MeetingRequestStatusTypes.Found;

    public void MarkAsSearching()
    {
      Status = MeetingRequestStatusTypes.Searching;
      ModifiedDate = DateTimeOffset.UtcNow;
    }

    public void MarkAsFound()
    {
      Status = MeetingRequestStatusTypes.Found;
      ModifiedDate = DateTimeOffset.UtcNow;
    }

    public void Abort()
    {
      IsRemoved = true;
      RemovedDate = DateTimeOffset.UtcNow;
      RemovedReason = MeetingRequestRemovedReasonTypes.Aborted;
    }

    public void Expire()
    {
      IsRemoved = true;
      RemovedDate = DateTimeOffset.UtcNow;
      RemovedReason = MeetingRequestRemovedReasonTypes.Expired;
    }
  }
}

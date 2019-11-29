using System;
using Skelvy.Domain.Entities.Core;
using Skelvy.Domain.Enums;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Meeting : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public Meeting(DateTimeOffset date, double latitude, double longitude, int size, bool isPrivate, bool isHidden, int groupId, int activityId)
    {
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      Size = size;
      IsPrivate = isPrivate;
      IsHidden = isHidden;
      GroupId = groupId;
      ActivityId = activityId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Size { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsHidden { get; set; }
    public int GroupId { get; set; }
    public int ActivityId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public string RemovedReason { get; set; }

    public Group Group { get; set; }
    public Activity Activity { get; set; }

    public void Update(DateTimeOffset date, double latitude, double longitude, int size, bool isHidden)
    {
      Date = date >= DateTimeOffset.UtcNow.AddDays(-1)
        ? date
        : throw new DomainException($"'Date' must show the future for {nameof(Meeting)}({Id}).");

      Latitude = latitude;
      Longitude = longitude;
      Size = size;
      IsHidden = isHidden;

      ModifiedAt = DateTimeOffset.UtcNow;
    }

    public void Abort()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingRemovedReasonType.Aborted;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(Meeting)}({Id}) is already aborted.");
      }
    }

    public void Expire()
    {
      if (!IsRemoved)
      {
        IsRemoved = true;
        RemovedReason = MeetingRemovedReasonType.Expired;
        ModifiedAt = DateTimeOffset.UtcNow;
      }
      else
      {
        throw new DomainException($"{nameof(Meeting)}({Id}) is already expired.");
      }
    }
  }
}

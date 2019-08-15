using System;
using Skelvy.Domain.Entities.Base;
using Skelvy.Domain.Enums.Meetings;
using Skelvy.Domain.Exceptions;

namespace Skelvy.Domain.Entities
{
  public class Meeting : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public Meeting(DateTimeOffset date, double latitude, double longitude, bool isPrivate, bool isHidden, int groupId, int activityId)
    {
      Date = date;
      Latitude = latitude;
      Longitude = longitude;
      IsPrivate = isPrivate;
      IsHidden = isHidden;
      GroupId = groupId;
      ActivityId = activityId;

      CreatedAt = DateTimeOffset.UtcNow;
    }

    public Meeting(DateTimeOffset date, double latitude, double longitude, int groupId, int activityId)
      : this(date, latitude, longitude, false, false, groupId, activityId)
    {
    }

    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
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
        throw new DomainException($"Entity {nameof(Meeting)}(Id = {Id}) is already aborted.");
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
        throw new DomainException($"Entity {nameof(Meeting)}(Id = {Id}) is already expired.");
      }
    }
  }
}

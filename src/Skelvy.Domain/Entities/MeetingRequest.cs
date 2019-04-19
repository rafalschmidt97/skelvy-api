using System;
using System.Collections.Generic;
using Skelvy.Domain.Entities.Base;

namespace Skelvy.Domain.Entities
{
  public class MeetingRequest : ICreatableEntity, IModifiableEntity, IRemovableEntity
  {
    public MeetingRequest()
    {
      CreatedDate = DateTimeOffset.UtcNow;
      Drinks = new List<MeetingRequestDrink>();
    }

    public int Id { get; set; }
    public string Status { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? ModifiedDate { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? RemovedDate { get; set; }
    public string RemovedReason { get; set; }
    public int UserId { get; set; }

    public IList<MeetingRequestDrink> Drinks { get; private set; }
    public User User { get; set; }

    public void Update()
    {
      ModifiedDate = DateTimeOffset.UtcNow;
    }

    public void UpdateStatus(string status)
    {
      Status = status;
      Update();
    }

    public void Remove(string reason)
    {
      IsRemoved = true;
      RemovedDate = DateTimeOffset.UtcNow;
      RemovedReason = reason;
    }
  }
}

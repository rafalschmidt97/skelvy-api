using System;
using System.Collections.Generic;

namespace Skelvy.Domain.Entities
{
  public class MeetingRequest
  {
    public MeetingRequest()
    {
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
    public int UserId { get; set; }

    public IList<MeetingRequestDrink> Drinks { get; private set; }
    public User User { get; set; }
  }
}

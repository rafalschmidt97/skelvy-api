using System;
using System.Collections.Generic;

namespace Skelvy.Domain.Entities
{
  public class Meeting
  {
    public Meeting()
    {
      Users = new List<MeetingUser>();
      ChatMessages = new List<MeetingChatMessage>();
    }

    public int Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int DrinkId { get; set; }

    public IList<MeetingUser> Users { get; private set; }
    public IList<MeetingChatMessage> ChatMessages { get; private set; }
    public Drink Drink { get; set; }
  }
}

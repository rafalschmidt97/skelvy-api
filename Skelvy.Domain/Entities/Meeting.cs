using System;
using System.Collections.Generic;

namespace Skelvy.Domain.Entities
{
  public class Meeting
  {
    public Meeting()
    {
      Users = new HashSet<MeetingUser>();
      ChatMessages = new HashSet<MeetingChatMessage>();
    }

    public int Id { get; set; }
    public DateTime Date { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int DrinkId { get; set; }

    public ICollection<MeetingUser> Users { get; private set; }
    public ICollection<MeetingChatMessage> ChatMessages { get; private set; }
    public Drink Drink { get; set; }
  }
}

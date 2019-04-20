using System;
using System.Collections.Generic;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommand : ICommand
  {
    public CreateMeetingRequestCommand(
      int userId,
      DateTimeOffset minDate,
      DateTimeOffset maxDate,
      int minAge,
      int maxAge,
      double latitude,
      double longitude,
      IList<CreateMeetingRequestDrink> drinks)
    {
      UserId = userId;
      MinDate = minDate;
      MaxDate = maxDate;
      MinAge = minAge;
      MaxAge = maxAge;
      Latitude = latitude;
      Longitude = longitude;
      Drinks = drinks;
    }

    public int UserId { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public IList<CreateMeetingRequestDrink> Drinks { get; set; }
  }

  public class CreateMeetingRequestDrink
  {
    public CreateMeetingRequestDrink(int id)
    {
      Id = id;
    }

    public int Id { get; set; }
  }
}

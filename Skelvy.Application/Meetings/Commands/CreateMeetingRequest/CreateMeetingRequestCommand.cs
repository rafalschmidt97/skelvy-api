using System;
using System.Collections.Generic;
using MediatR;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommand : IRequest
  {
    public int UserId { get; set; }
    public DateTimeOffset MinDate { get; set; }
    public DateTimeOffset MaxDate { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public ICollection<CreateMeetingRequestDrink> Drinks { get; set; }
  }

  public class CreateMeetingRequestDrink
  {
    public int Id { get; set; }
  }
}

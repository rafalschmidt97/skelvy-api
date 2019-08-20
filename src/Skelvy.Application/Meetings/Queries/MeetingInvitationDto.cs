using System;
using Skelvy.Application.Users.Queries;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingInvitationDto
  {
    public int Id { get; set; }
    public UserDto InvitingUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

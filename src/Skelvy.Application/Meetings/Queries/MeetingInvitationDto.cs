using System;
using Skelvy.Application.Users.Queries;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingInvitationDto
  {
    public int Id { get; set; }
    public int InvitingUserId { get; set; }
    public UserDto InvitedUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }

  public class SelfMeetingInvitationDto
  {
    public int Id { get; set; }
    public int InvitingUserId { get; set; }
    public MeetingWithUsersDto Meeting { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

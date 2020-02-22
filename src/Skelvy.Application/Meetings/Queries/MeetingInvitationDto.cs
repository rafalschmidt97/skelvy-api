using System;
using Skelvy.Application.Core.Mappers;
using Skelvy.Application.Users.Queries;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Queries
{
  public class MeetingInvitationDto : IMapping<MeetingInvitation>
  {
    public int Id { get; set; }
    public int InvitingUserId { get; set; }
    public UserDto InvitedUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }

  public class SelfMeetingInvitationDto : IMapping<MeetingInvitation>
  {
    public int Id { get; set; }
    public int InvitingUserId { get; set; }
    public MeetingWithUsersDto Meeting { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

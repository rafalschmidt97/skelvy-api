using System;
using Skelvy.Application.Users.Queries;

namespace Skelvy.Application.Relations.Queries
{
  public class FriendInvitationsDto
  {
    public int Id { get; set; }
    public UserDto InvitingUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

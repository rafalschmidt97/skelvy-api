using System;
using Skelvy.Application.Users.Queries;

namespace Skelvy.Application.Relations.Queries
{
  public class FriendRequestDto
  {
    public FriendRequestDto(int id, UserDto invitingUser, DateTimeOffset createdAt)
    {
      Id = id;
      InvitingUser = invitingUser;
      CreatedAt = createdAt;
    }

    public int Id { get; set; }
    public UserDto InvitingUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

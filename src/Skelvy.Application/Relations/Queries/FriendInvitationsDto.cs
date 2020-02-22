using System;
using Skelvy.Application.Core.Mappers;
using Skelvy.Application.Users.Queries;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Relations.Queries
{
  public class FriendInvitationsDto : IMapping<FriendInvitation>
  {
    public int Id { get; set; }
    public UserDto InvitingUser { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

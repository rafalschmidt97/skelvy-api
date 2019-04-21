using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.RemoveUsers
{
  public class RemoveUsersCommandHandler : CommandHandler<RemoveUsersCommand>
  {
    private readonly SkelvyContext _context;

    public RemoveUsersCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public override async Task<Unit> Handle(RemoveUsersCommand request)
    {
      var today = DateTimeOffset.UtcNow;

      var usersToRemove = await _context.Users.Where(x => x.IsRemoved && x.ForgottenAt < today).ToListAsync();
      _context.Users.RemoveRange(usersToRemove);

      var usersId = usersToRemove.Select(x => x.Id);

      var userRolesToRemove = await _context.UserRoles.Where(x => usersId.Any(y => y == x.UserId)).ToListAsync();
      _context.UserRoles.RemoveRange(userRolesToRemove);

      var userProfilesToRemove = await _context.UserProfiles.Where(x => usersId.Any(y => y == x.UserId)).ToListAsync();
      _context.UserProfiles.RemoveRange(userProfilesToRemove);

      var userProfilesId = userProfilesToRemove.Select(y => y.Id);
      var userProfilePhotosToRemove = await _context.UserProfilePhotos.Where(x => userProfilesId.Any(y => y == x.ProfileId)).ToListAsync();
      _context.UserProfilePhotos.RemoveRange(userProfilePhotosToRemove);

      var meetingRequestsToRemove = await _context.MeetingRequests.Where(x => usersId.Any(y => y == x.UserId)).ToListAsync();
      _context.MeetingRequests.RemoveRange(meetingRequestsToRemove);

      var meetingRequestsId = meetingRequestsToRemove.Select(y => y.Id);
      var meetingRequestDrinksToRemove = await _context.MeetingRequestDrinks.Where(x => meetingRequestsId.Any(y => y == x.MeetingRequestId)).ToListAsync();
      _context.MeetingRequestDrinks.RemoveRange(meetingRequestDrinksToRemove);

      var meetingUsersToRemove = await _context.MeetingUsers.Where(x => usersId.Any(y => y == x.UserId)).ToListAsync();
      _context.MeetingUsers.RemoveRange(meetingUsersToRemove);

      var messagesToRemove = await _context.MeetingChatMessages.Where(x => usersId.Any(y => y == x.UserId)).ToListAsync();
      _context.MeetingChatMessages.RemoveRange(messagesToRemove);

      await _context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}

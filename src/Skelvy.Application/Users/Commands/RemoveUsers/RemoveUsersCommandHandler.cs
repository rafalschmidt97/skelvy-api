using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;

namespace Skelvy.Application.Users.Commands.RemoveUsers
{
  public class RemoveUsersCommandHandler : CommandHandler<RemoveUsersCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IAuthRolesRepository _rolesRepository;
    private readonly IUserProfilesRepository _profilesRepository;
    private readonly IUserProfilePhotosRepository _profilePhotosRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingRequestDrinksRepository _requestDrinksRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingChatMessagesRepository _messagesRepository;

    public RemoveUsersCommandHandler(
      IUsersRepository usersRepository,
      IAuthRolesRepository rolesRepository,
      IUserProfilesRepository profilesRepository,
      IUserProfilePhotosRepository profilePhotosRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingRequestDrinksRepository requestDrinksRepository,
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingChatMessagesRepository messagesRepository)
    {
      _usersRepository = usersRepository;
      _rolesRepository = rolesRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _requestsRepository = requestsRepository;
      _requestDrinksRepository = requestDrinksRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _messagesRepository = messagesRepository;
    }

    public override async Task<Unit> Handle(RemoveUsersCommand request)
    {
      var today = DateTimeOffset.UtcNow;

      var usersToRemove = await _usersRepository.FindAllRemovedAfterForgottenAt(today);
      _usersRepository.Context.Users.RemoveRange(usersToRemove);

      var usersId = usersToRemove.Select(x => x.Id).ToList();

      var userRolesToRemove = await _rolesRepository.FindAllByUsersId(usersId);
      _rolesRepository.Context.UserRoles.RemoveRange(userRolesToRemove);

      var userProfilesToRemove = await _profilesRepository.FindAllByUsersId(usersId);
      _profilesRepository.Context.UserProfiles.RemoveRange(userProfilesToRemove);

      var userProfilesId = userProfilesToRemove.Select(y => y.Id).ToList();
      var userProfilePhotosToRemove = await _profilePhotosRepository.FindAllWithRemovedByProfilesId(userProfilesId);
      _profilePhotosRepository.Context.UserProfilePhotos.RemoveRange(userProfilePhotosToRemove);

      var meetingRequestsToRemove = await _requestsRepository.FindAllWithRemovedByUsersId(usersId);
      _requestsRepository.Context.MeetingRequests.RemoveRange(meetingRequestsToRemove);

      var meetingRequestsId = meetingRequestsToRemove.Select(y => y.Id);
      var meetingRequestDrinksToRemove = await _requestDrinksRepository.FindAllByRequestsId(meetingRequestsId);
      _requestDrinksRepository.Context.MeetingRequestDrinks.RemoveRange(meetingRequestDrinksToRemove);

      var meetingUsersToRemove = await _meetingUsersRepository.FindAllWithRemovedByUsersId(usersId);
      _meetingUsersRepository.Context.MeetingUsers.RemoveRange(meetingUsersToRemove);

      var messagesToRemove = await _messagesRepository.FindAllByUsersId(usersId);
      _messagesRepository.Context.MeetingChatMessages.RemoveRange(messagesToRemove);

      await _usersRepository.Context.SaveChangesAsync();

      return Unit.Value;
    }
  }
}

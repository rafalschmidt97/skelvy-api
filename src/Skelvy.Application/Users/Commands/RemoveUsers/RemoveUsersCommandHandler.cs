using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;

namespace Skelvy.Application.Users.Commands.RemoveUsers
{
  public class RemoveUsersCommandHandler : CommandHandler<RemoveUsersCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IUserRolesRepository _rolesRepository;
    private readonly IUserProfilesRepository _profilesRepository;
    private readonly IUserProfilePhotosRepository _profilePhotosRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingRequestDrinkTypesRepository _requestDrinkTypesRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingChatMessagesRepository _messagesRepository;
    private readonly IBlockedUsersRepository _blockedUsersRepository;

    public RemoveUsersCommandHandler(
      IUsersRepository usersRepository,
      IUserRolesRepository rolesRepository,
      IUserProfilesRepository profilesRepository,
      IUserProfilePhotosRepository profilePhotosRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingRequestDrinkTypesRepository requestDrinkTypesRepository,
      IMeetingUsersRepository meetingUsersRepository,
      IMeetingChatMessagesRepository messagesRepository,
      IBlockedUsersRepository blockedUsersRepository)
    {
      _usersRepository = usersRepository;
      _rolesRepository = rolesRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _requestsRepository = requestsRepository;
      _requestDrinkTypesRepository = requestDrinkTypesRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _messagesRepository = messagesRepository;
      _blockedUsersRepository = blockedUsersRepository;
    }

    public override async Task<Unit> Handle(RemoveUsersCommand request)
    {
      var today = DateTimeOffset.UtcNow;

      using (var transaction = _usersRepository.BeginTransaction())
      {
        var usersToRemove = await _usersRepository.FindAllRemovedAfterForgottenAt(today);
        var usersId = usersToRemove.Select(x => x.Id).ToList();

        var messagesToRemove = await _messagesRepository.FindAllByUsersId(usersId);
        await _messagesRepository.RemoveRange(messagesToRemove);

        var meetingUsersToRemove = await _meetingUsersRepository.FindAllWithRemovedByUsersId(usersId);
        await _meetingUsersRepository.RemoveRange(meetingUsersToRemove);

        var meetingRequestsToRemove = await _requestsRepository.FindAllWithRemovedByUsersId(usersId);
        var meetingRequestsId = meetingRequestsToRemove.Select(y => y.Id);
        var meetingRequestDrinkTypesToRemove = await _requestDrinkTypesRepository.FindAllByRequestsId(meetingRequestsId);
        await _requestDrinkTypesRepository.RemoveRange(meetingRequestDrinkTypesToRemove);
        await _requestsRepository.RemoveRange(meetingRequestsToRemove);

        var userProfilesToRemove = await _profilesRepository.FindAllByUsersId(usersId);
        var userProfilesId = userProfilesToRemove.Select(y => y.Id).ToList();
        var userProfilePhotosToRemove = await _profilePhotosRepository.FindAllWithRemovedByProfilesId(userProfilesId);
        await _profilePhotosRepository.RemoveRange(userProfilePhotosToRemove);
        await _profilesRepository.RemoveRange(userProfilesToRemove);

        var blockedUsersToRemove = await _blockedUsersRepository.FindAllWithRemovedByUsersId(usersId);
        await _blockedUsersRepository.RemoveRange(blockedUsersToRemove);

        var userRolesToRemove = await _rolesRepository.FindAllByUsersId(usersId);
        await _rolesRepository.RemoveRange(userRolesToRemove);

        await _usersRepository.RemoveRange(usersToRemove);

        transaction.Commit();
      }

      return Unit.Value;
    }
  }
}

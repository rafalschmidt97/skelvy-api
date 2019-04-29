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

      using (var transaction = _usersRepository.BeginTransaction())
      {
        var usersToRemove = await _usersRepository.FindAllRemovedAfterForgottenAt(today);
        await _usersRepository.RemoveRange(usersToRemove);

        var usersId = usersToRemove.Select(x => x.Id).ToList();

        var userRolesToRemove = await _rolesRepository.FindAllByUsersId(usersId);
        await _rolesRepository.RemoveRange(userRolesToRemove);

        var userProfilesToRemove = await _profilesRepository.FindAllByUsersId(usersId);
        await _profilesRepository.RemoveRange(userProfilesToRemove);

        var userProfilesId = userProfilesToRemove.Select(y => y.Id).ToList();
        var userProfilePhotosToRemove = await _profilePhotosRepository.FindAllWithRemovedByProfilesId(userProfilesId);
        await _profilePhotosRepository.RemoveRange(userProfilePhotosToRemove);

        var meetingRequestsToRemove = await _requestsRepository.FindAllWithRemovedByUsersId(usersId);
        await _requestsRepository.RemoveRange(meetingRequestsToRemove);

        var meetingRequestsId = meetingRequestsToRemove.Select(y => y.Id);
        var meetingRequestDrinksToRemove = await _requestDrinksRepository.FindAllByRequestsId(meetingRequestsId);
        await _requestDrinksRepository.RemoveRange(meetingRequestDrinksToRemove);

        var meetingUsersToRemove = await _meetingUsersRepository.FindAllWithRemovedByUsersId(usersId);
        await _meetingUsersRepository.RemoveRange(meetingUsersToRemove);

        var messagesToRemove = await _messagesRepository.FindAllByUsersId(usersId);
        await _messagesRepository.RemoveRange(messagesToRemove);

        transaction.Commit();
      }

      return Unit.Value;
    }
  }
}

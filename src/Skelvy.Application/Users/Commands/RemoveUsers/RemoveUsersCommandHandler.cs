using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Uploads.Infrastructure.Repositories;
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
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IAttachmentsRepository _attachmentsRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IFriendRequestsRepository _friendRequestsRepository;

    public RemoveUsersCommandHandler(
      IUsersRepository usersRepository,
      IUserRolesRepository rolesRepository,
      IUserProfilesRepository profilesRepository,
      IUserProfilePhotosRepository profilePhotosRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingRequestDrinkTypesRepository requestDrinkTypesRepository,
      IGroupUsersRepository groupUsersRepository,
      IMessagesRepository messagesRepository,
      IAttachmentsRepository attachmentsRepository,
      IRelationsRepository relationsRepository,
      IFriendRequestsRepository friendRequestsRepository)
    {
      _usersRepository = usersRepository;
      _rolesRepository = rolesRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _requestsRepository = requestsRepository;
      _requestDrinkTypesRepository = requestDrinkTypesRepository;
      _groupUsersRepository = groupUsersRepository;
      _messagesRepository = messagesRepository;
      _attachmentsRepository = attachmentsRepository;
      _relationsRepository = relationsRepository;
      _friendRequestsRepository = friendRequestsRepository;
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

        var meetingUsersToRemove = await _groupUsersRepository.FindAllWithRemovedByUsersId(usersId);
        await _groupUsersRepository.RemoveRange(meetingUsersToRemove);

        var meetingRequestsToRemove = await _requestsRepository.FindAllWithRemovedByUsersId(usersId);
        var meetingRequestsId = meetingRequestsToRemove.Select(y => y.Id);
        var meetingRequestDrinkTypesToRemove = await _requestDrinkTypesRepository.FindAllByRequestsId(meetingRequestsId);
        await _requestDrinkTypesRepository.RemoveRange(meetingRequestDrinkTypesToRemove);
        await _requestsRepository.RemoveRange(meetingRequestsToRemove);

        var userProfilesToRemove = await _profilesRepository.FindAllByUsersId(usersId);
        var userProfilesId = userProfilesToRemove.Select(y => y.Id).ToList();
        var userProfilePhotosToRemove = await _profilePhotosRepository.FindAllWithRemovedByProfilesId(userProfilesId);
        var attachmentsId = userProfilePhotosToRemove.Select(y => y.AttachmentId).ToList();
        var attachmentsToRemove = await _attachmentsRepository.FindAllByAttachmentsId(attachmentsId);
        await _attachmentsRepository.RemoveRange(attachmentsToRemove);
        await _profilePhotosRepository.RemoveRange(userProfilePhotosToRemove);
        await _profilesRepository.RemoveRange(userProfilesToRemove);

        var relationsToRemove = await _relationsRepository.FindAllWithRemovedByUsersId(usersId);
        await _relationsRepository.RemoveRange(relationsToRemove);
        var friendRequestsToRemove = await _friendRequestsRepository.FindAllWithRemovedByUsersId(usersId);
        await _friendRequestsRepository.RemoveRange(friendRequestsToRemove);

        var userRolesToRemove = await _rolesRepository.FindAllByUsersId(usersId);
        await _rolesRepository.RemoveRange(userRolesToRemove);

        await _usersRepository.RemoveRange(usersToRemove);

        transaction.Commit();
      }

      return Unit.Value;
    }
  }
}

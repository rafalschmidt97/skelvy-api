using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Messages.Infrastructure.Repositories;
using Skelvy.Application.Relations.Infrastructure.Repositories;
using Skelvy.Application.Uploads.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;

namespace Skelvy.Application.Users.Commands.RemoveUsers
{
  public class RemoveUsersCommandHandler : CommandHandler<RemoveUsersCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IUserRolesRepository _rolesRepository;
    private readonly IProfilesRepository _profilesRepository;
    private readonly IProfilePhotosRepository _profilePhotosRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingRequestActivityRepository _requestActivityRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IAttachmentsRepository _attachmentsRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IFriendRequestsRepository _friendRequestsRepository;
    private readonly IMeetingInvitationsRepository _invitationsRepository;

    public RemoveUsersCommandHandler(
      IUsersRepository usersRepository,
      IUserRolesRepository rolesRepository,
      IProfilesRepository profilesRepository,
      IProfilePhotosRepository profilePhotosRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingRequestActivityRepository requestActivityRepository,
      IGroupUsersRepository groupUsersRepository,
      IMessagesRepository messagesRepository,
      IAttachmentsRepository attachmentsRepository,
      IRelationsRepository relationsRepository,
      IFriendRequestsRepository friendRequestsRepository,
      IMeetingInvitationsRepository invitationsRepository)
    {
      _usersRepository = usersRepository;
      _rolesRepository = rolesRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _requestsRepository = requestsRepository;
      _requestActivityRepository = requestActivityRepository;
      _groupUsersRepository = groupUsersRepository;
      _messagesRepository = messagesRepository;
      _attachmentsRepository = attachmentsRepository;
      _relationsRepository = relationsRepository;
      _friendRequestsRepository = friendRequestsRepository;
      _invitationsRepository = invitationsRepository;
    }

    public override async Task<Unit> Handle(RemoveUsersCommand request)
    {
      var today = DateTimeOffset.UtcNow;

      using (var transaction = _usersRepository.BeginTransaction())
      {
        var usersToRemove = await _usersRepository.FindAllRemovedAfterForgottenAtByDate(today);
        var usersId = usersToRemove.Select(x => x.Id).ToList();

        var messagesToRemove = await _messagesRepository.FindAllByUsersId(usersId);
        await _messagesRepository.RemoveRange(messagesToRemove);

        var groupUsersToRemove = await _groupUsersRepository.FindAllWithRemovedByUsersId(usersId);
        await _groupUsersRepository.RemoveRange(groupUsersToRemove);

        var meetingRequestsToRemove = await _requestsRepository.FindAllWithRemovedByUsersId(usersId);
        var meetingRequestsId = meetingRequestsToRemove.Select(y => y.Id);
        var meetingRequestActivitiesToRemove = await _requestActivityRepository.FindAllByRequestsId(meetingRequestsId);
        await _requestActivityRepository.RemoveRange(meetingRequestActivitiesToRemove);
        await _requestsRepository.RemoveRange(meetingRequestsToRemove);

        var profilesToRemove = await _profilesRepository.FindAllByUsersId(usersId);
        var profilesId = profilesToRemove.Select(y => y.Id).ToList();
        var profilePhotosToRemove = await _profilePhotosRepository.FindAllWithRemovedByProfilesId(profilesId);
        var attachmentsId = profilePhotosToRemove.Select(y => y.AttachmentId).ToList();
        var attachmentsToRemove = await _attachmentsRepository.FindAllByAttachmentsId(attachmentsId);
        await _attachmentsRepository.RemoveRange(attachmentsToRemove);
        await _profilePhotosRepository.RemoveRange(profilePhotosToRemove);
        await _profilesRepository.RemoveRange(profilesToRemove);

        var relationsToRemove = await _relationsRepository.FindAllWithRemovedByUsersId(usersId);
        await _relationsRepository.RemoveRange(relationsToRemove);
        var friendRequestsToRemove = await _friendRequestsRepository.FindAllWithRemovedByUsersId(usersId);
        await _friendRequestsRepository.RemoveRange(friendRequestsToRemove);

        var meetingInvitationsToRemove = await _invitationsRepository.FindAllWithRemovedByUsersId(usersId);
        await _invitationsRepository.RemoveRange(meetingInvitationsToRemove);

        var userRolesToRemove = await _rolesRepository.FindAllByUsersId(usersId);
        await _rolesRepository.RemoveRange(userRolesToRemove);

        await _usersRepository.RemoveRange(usersToRemove);

        transaction.Commit();
      }

      return Unit.Value;
    }
  }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Auth.Infrastructure.Repositories;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
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
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IProfilesRepository _profilesRepository;
    private readonly IProfilePhotosRepository _profilePhotosRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IMeetingRequestActivityRepository _requestActivityRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMessagesRepository _messagesRepository;
    private readonly IAttachmentsRepository _attachmentsRepository;
    private readonly IRelationsRepository _relationsRepository;
    private readonly IFriendInvitationsRepository _friendInvitationsRepository;
    private readonly IMeetingInvitationsRepository _invitationsRepository;

    public RemoveUsersCommandHandler(
      IUsersRepository usersRepository,
      IUserRolesRepository rolesRepository,
      IRefreshTokenRepository refreshTokenRepository,
      IProfilesRepository profilesRepository,
      IProfilePhotosRepository profilePhotosRepository,
      IMeetingRequestsRepository requestsRepository,
      IMeetingRequestActivityRepository requestActivityRepository,
      IGroupUsersRepository groupUsersRepository,
      IMessagesRepository messagesRepository,
      IAttachmentsRepository attachmentsRepository,
      IRelationsRepository relationsRepository,
      IFriendInvitationsRepository friendInvitationsRepository,
      IMeetingInvitationsRepository invitationsRepository)
    {
      _usersRepository = usersRepository;
      _rolesRepository = rolesRepository;
      _refreshTokenRepository = refreshTokenRepository;
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _requestsRepository = requestsRepository;
      _requestActivityRepository = requestActivityRepository;
      _groupUsersRepository = groupUsersRepository;
      _messagesRepository = messagesRepository;
      _attachmentsRepository = attachmentsRepository;
      _relationsRepository = relationsRepository;
      _friendInvitationsRepository = friendInvitationsRepository;
      _invitationsRepository = invitationsRepository;
    }

    public override async Task<Unit> Handle(RemoveUsersCommand request)
    {
      var today = DateTimeOffset.UtcNow;

      await using var transaction = _usersRepository.BeginTransaction();
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
      await _profilePhotosRepository.RemoveRange(profilePhotosToRemove);
      await _attachmentsRepository.RemoveRange(attachmentsToRemove);
      await _profilesRepository.RemoveRange(profilesToRemove);

      var relationsToRemove = await _relationsRepository.FindAllWithRemovedByUsersId(usersId);
      await _relationsRepository.RemoveRange(relationsToRemove);
      var friendInvitationsToRemove = await _friendInvitationsRepository.FindAllWithRemovedByUsersId(usersId);
      await _friendInvitationsRepository.RemoveRange(friendInvitationsToRemove);
      var meetingInvitationsToRemove = await _invitationsRepository.FindAllWithRemovedByUsersId(usersId);
      await _invitationsRepository.RemoveRange(meetingInvitationsToRemove);

      var refreshTokensToRemove = await _refreshTokenRepository.FindAllByUsersId(usersId);
      await _refreshTokenRepository.RemoveRange(refreshTokensToRemove);

      var userRolesToRemove = await _rolesRepository.FindAllByUsersId(usersId);
      await _rolesRepository.RemoveRange(userRolesToRemove);

      await _usersRepository.RemoveRange(usersToRemove);

      transaction.Commit();

      return Unit.Value;
    }
  }
}

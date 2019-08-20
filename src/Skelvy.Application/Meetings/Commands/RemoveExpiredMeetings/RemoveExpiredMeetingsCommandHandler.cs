using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.MeetingsExpired;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Extensions;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetings
{
  public class RemoveExpiredMeetingsCommandHandler : CommandHandler<RemoveExpiredMeetingsCommand>
  {
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingInvitationsRepository _meetingInvitationsRepository;
    private readonly IMediator _mediator;

    public RemoveExpiredMeetingsCommandHandler(
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingInvitationsRepository meetingInvitationsRepository,
      IMediator mediator)
    {
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingInvitationsRepository = meetingInvitationsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(RemoveExpiredMeetingsCommand request)
    {
      var yesterday = DateTimeOffset.UtcNow.AddDays(-1);
      var meetingsToRemove = await _meetingsRepository.FindAllAfterOrEqualDate(yesterday);

      if (meetingsToRemove.Any())
      {
        using (var transaction = _meetingRequestsRepository.BeginTransaction())
        {
          var groupsId = meetingsToRemove.Select(x => x.GroupId).ToList();
          var meetingsId = meetingsToRemove.Select(x => x.Id).ToList();
          var groupUsers = await _groupUsersRepository.FindAllWithRequestByGroupsId(groupsId);

          meetingsToRemove.ForEach(x => x.Expire());
          await _meetingsRepository.UpdateRange(meetingsToRemove);
          var groupUsersRequests = groupUsers.Select(x => x.MeetingRequest).Where(x => x != null).ToList();
          groupUsersRequests.ForEach(x => x.Expire());
          await _meetingRequestsRepository.UpdateRange(groupUsersRequests);

          var invitations = await _meetingInvitationsRepository.FindAllByMeetingsId(meetingsId);

          foreach (var invitation in invitations)
          {
            invitation.Abort();
          }

          await _meetingInvitationsRepository.UpdateRange(invitations);

          transaction.Commit();
          await _mediator.Publish(new MeetingsExpiredEvent(meetingsToRemove.Select(x => x.Id)));
        }
      }

      return Unit.Value;
    }
  }
}

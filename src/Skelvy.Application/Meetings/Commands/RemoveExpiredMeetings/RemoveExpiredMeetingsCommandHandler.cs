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
    private readonly IMediator _mediator;

    public RemoveExpiredMeetingsCommandHandler(
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMediator mediator)
    {
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(RemoveExpiredMeetingsCommand request)
    {
      var yesterday = DateTimeOffset.UtcNow.AddDays(-1);
      var meetingsToRemove = await _meetingsRepository.FindAllAfterOrEqualDate(yesterday);

      using (var transaction = _meetingRequestsRepository.BeginTransaction())
      {
        var isDataChanged = false;

        if (meetingsToRemove.Any())
        {
          var meetingsId = meetingsToRemove.Select(x => x.Id);
          var groupUsers = await _groupUsersRepository.FindAllWithRequestByGroupsId(meetingsId);

          meetingsToRemove.ForEach(x => x.Expire());
          var groupUsersRequests = groupUsers.Select(x => x.MeetingRequest).Where(x => x != null).ToList();
          groupUsersRequests.ForEach(x => x.Expire());
          await _meetingRequestsRepository.UpdateRange(groupUsersRequests);

          isDataChanged = true;
        }

        if (isDataChanged)
        {
          await _meetingsRepository.UpdateRange(meetingsToRemove);
          transaction.Commit();
          await _mediator.Publish(new MeetingsExpiredEvent(meetingsToRemove.Select(x => x.Id)));
        }
      }

      return Unit.Value;
    }
  }
}

using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Extensions;

namespace Skelvy.Application.Meetings.Commands.RemoveExpiredMeetingRequests
{
  public class RemoveExpiredMeetingRequestsCommandHandler : CommandHandler<RemoveExpiredMeetingRequestsCommand>
  {
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMediator _mediator;

    public RemoveExpiredMeetingRequestsCommandHandler(IMeetingRequestsRepository meetingRequestsRepository, IMediator mediator)
    {
      _meetingRequestsRepository = meetingRequestsRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(RemoveExpiredMeetingRequestsCommand request)
    {
      var yesterday = DateTimeOffset.UtcNow.AddDays(-1);
      var requestsToRemove = await _meetingRequestsRepository.FindAllSearchingAfterOrEqualMaxDate(yesterday);

      var isDataChanged = false;

      if (requestsToRemove.Any())
      {
        requestsToRemove.ForEach(x => x.Expire());
        isDataChanged = true;
      }

      if (isDataChanged)
      {
        await _meetingRequestsRepository.UpdateRange(requestsToRemove);
        await _mediator.Publish(new MeetingRequestExpiredAction(requestsToRemove.Select(x => x.Id)));
      }

      return Unit.Value;
    }
  }
}

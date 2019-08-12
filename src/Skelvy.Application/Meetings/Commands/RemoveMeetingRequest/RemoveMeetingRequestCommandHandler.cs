using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommandHandler : CommandHandler<RemoveMeetingRequestCommand>
  {
    private readonly IMeetingRequestsRepository _requestsRepository;

    public RemoveMeetingRequestCommandHandler(IMeetingRequestsRepository requestsRepository)
    {
      _requestsRepository = requestsRepository;
    }

    public override async Task<Unit> Handle(RemoveMeetingRequestCommand request)
    {
      var meetingRequest = await _requestsRepository.FindOneSearchingByRequestId(request.RequestId);

      if (meetingRequest == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) not found.");
      }

      if (meetingRequest.IsFound)
      {
        throw new InternalServerErrorException(
          $"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) is marked as '{MeetingRequestStatusType.Found}' " +
          $"while {nameof(GroupUser)} does not exists");
      }

      meetingRequest.Abort();

      await _requestsRepository.Update(meetingRequest);
      return Unit.Value;
    }
  }
}

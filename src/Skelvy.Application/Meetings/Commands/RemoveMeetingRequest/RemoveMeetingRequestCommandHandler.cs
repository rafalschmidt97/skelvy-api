using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommandHandler : CommandHandler<RemoveMeetingRequestCommand>
  {
    private readonly IMeetingRequestsRepository _requestsRepository;
    private readonly IUsersRepository _usersRepository;

    public RemoveMeetingRequestCommandHandler(IMeetingRequestsRepository requestsRepository, IUsersRepository usersRepository)
    {
      _requestsRepository = requestsRepository;
      _usersRepository = usersRepository;
    }

    public override async Task<Unit> Handle(RemoveMeetingRequestCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meetingRequest = await _requestsRepository.FindOneSearchingByRequestId(request.RequestId);

      if (meetingRequest == null)
      {
        throw new NotFoundException(nameof(MeetingRequest), request.RequestId);
      }

      if (meetingRequest.IsFound)
      {
        throw new InternalServerErrorException(
          $"{nameof(MeetingRequest)}({request.RequestId}) is marked as '{MeetingRequestStatusType.Found}' " +
          $"while {nameof(GroupUser)} does not exist");
      }

      if (meetingRequest.UserId != request.UserId)
      {
        throw new ForbiddenException(
          $"{nameof(MeetingRequest)}({request.RequestId}) does not belong to {nameof(User)}({request.UserId}");
      }

      meetingRequest.Abort();

      await _requestsRepository.Update(meetingRequest);
      return Unit.Value;
    }
  }
}

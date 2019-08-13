using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Meetings;

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

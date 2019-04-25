using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.RemoveMeetingRequest
{
  public class RemoveMeetingRequestCommandHandler : CommandHandler<RemoveMeetingRequestCommand>
  {
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMeetingRequestsRepository _requestsRepository;

    public RemoveMeetingRequestCommandHandler(IMeetingUsersRepository meetingUsersRepository, IMeetingRequestsRepository requestsRepository)
    {
      _meetingUsersRepository = meetingUsersRepository;
      _requestsRepository = requestsRepository;
    }

    public override async Task<Unit> Handle(RemoveMeetingRequestCommand request)
    {
      var meetingUserExists = await _meetingUsersRepository.ExistsOneByUserId(request.UserId);

      if (meetingUserExists)
      {
        throw new ConflictException($"Entity {nameof(Meeting)}(UserId = {request.UserId}) exists. Leave meeting instead.");
      }

      var meetingRequest = await _requestsRepository.FindOneSearchingByUserId(request.UserId);

      if (meetingRequest == null)
      {
        throw new NotFoundException($"Entity {nameof(MeetingRequest)}(UserId = {request.UserId}) not found.");
      }

      meetingRequest.Abort();

      await _requestsRepository.Context.SaveChangesAsync();
      return Unit.Value;
    }
  }
}

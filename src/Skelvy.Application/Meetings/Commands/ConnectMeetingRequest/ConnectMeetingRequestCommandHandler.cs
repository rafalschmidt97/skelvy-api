using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.UserConnectedToMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.ConnectMeetingRequest
{
  public class ConnectMeetingRequestCommandHandler : CommandHandler<ConnectMeetingRequestCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<ConnectMeetingRequestCommandHandler> _logger;

    public ConnectMeetingRequestCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator,
      ILogger<ConnectMeetingRequestCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
      _logger = logger;
    }

    public override async Task<Unit> Handle(ConnectMeetingRequestCommand request)
    {
      var user = await _usersRepository.FindOneWithDetails(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var connectingMeetingRequest =
        await _meetingRequestsRepository.FindOneSearchingWithUserDetailsByRequestId(request.MeetingRequestId);

      if (connectingMeetingRequest == null)
      {
        throw new NotFoundException(nameof(MeetingRequest), request.MeetingRequestId);
      }

      using (var transaction = _groupUsersRepository.BeginTransaction())
      {
        try
        {
          var meeting = await CreateNewMeeting(user, connectingMeetingRequest);
          transaction.Commit();
          await _mediator.Publish(new UserConnectedToMeetingEvent(connectingMeetingRequest.UserId, meeting.Id));
        }
        catch (Exception exception)
        {
          _logger.LogError(
            $"{nameof(ConnectMeetingRequestCommand)} Exception while CreateNewMeetingRequest/CreateNewMeeting for " +
            $"User(Id={user.Id}): {exception.Message}");
        }
      }

      return Unit.Value;
    }

    private async Task<Meeting> CreateNewMeeting(User user, MeetingRequest request)
    {
      var group = new Group();
      await _groupsRepository.Add(group);

      var meeting = new Meeting(
        request.MinDate,
        request.Latitude,
        request.Longitude,
        group.Id,
        request.Activities[0].ActivityId);

      await _meetingsRepository.Add(meeting);

      var groupUsers = new[]
      {
        new GroupUser(group.Id, user.Id),
        new GroupUser(group.Id, request.UserId, request.Id),
      };

      await _groupUsersRepository.AddRange(groupUsers);

      request.MarkAsFound();
      await _meetingRequestsRepository.Update(request);

      return meeting;
    }
  }
}

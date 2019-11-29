using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Events.UserConnectedToMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

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
      var (user, connectingMeetingRequest) = await ValidateData(request);

      using (var transaction = _groupUsersRepository.BeginTransaction())
      {
        try
        {
          var meeting = await CreateNewMeeting(user, connectingMeetingRequest, request);
          transaction.Commit();
          await _mediator.Publish(new UserConnectedToMeetingEvent(connectingMeetingRequest.UserId, meeting.Id, meeting.GroupId));
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

    private async Task<(User, MeetingRequest)> ValidateData(ConnectMeetingRequestCommand request)
    {
      var user = await _usersRepository.FindOneWithDetails(request.UserId);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var connectingMeetingRequest = await _meetingRequestsRepository
          .FindOneSearchingWithActivitiesByRequestIdAndUserId(request.MeetingRequestId, request.UserId);

      if (connectingMeetingRequest == null)
      {
        throw new NotFoundException(nameof(MeetingRequest), request.MeetingRequestId);
      }

      if (connectingMeetingRequest.UserId == request.UserId)
      {
        throw new ConflictException($"{nameof(MeetingRequest)}({request.MeetingRequestId} must be be non self.");
      }

      if (connectingMeetingRequest.Activities.All(x => x.ActivityId != request.ActivityId))
      {
        throw new BadRequestException("Activity must be selected from request preferences.");
      }

      if (request.Date < connectingMeetingRequest.MinDate || request.Date > connectingMeetingRequest.MaxDate)
      {
        throw new BadRequestException("Date must be between request preferences.");
      }

      return (user, connectingMeetingRequest);
    }

    private async Task<Meeting> CreateNewMeeting(User user, MeetingRequest meetingRequest, ConnectMeetingRequestCommand request)
    {
      var group = new Group();
      await _groupsRepository.Add(group);

      var size = meetingRequest.Activities.First(x => x.ActivityId == request.ActivityId).Activity.Size;

      var meeting = new Meeting(
        request.Date,
        meetingRequest.Latitude,
        meetingRequest.Longitude,
        size,
        false,
        false,
        group.Id,
        request.ActivityId);

      await _meetingsRepository.Add(meeting);

      var groupUsers = new[]
      {
        new GroupUser(group.Id, user.Id, GroupUserRoleType.Admin),
        new GroupUser(group.Id, meetingRequest.UserId, meetingRequest.Id, GroupUserRoleType.Admin),
      };

      await _groupUsersRepository.AddRange(groupUsers);

      meetingRequest.MarkAsFound();
      await _meetingRequestsRepository.Update(meetingRequest);

      return meeting;
    }
  }
}

using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.UserConnectedToMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Extensions;

namespace Skelvy.Application.Meetings.Commands.ConnectMeetingRequest
{
  public class ConnectMeetingRequestCommandHandler : CommandHandler<ConnectMeetingRequestCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingRequestActivityRepository _meetingRequestActivityRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<ConnectMeetingRequestCommandHandler> _logger;

    public ConnectMeetingRequestCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingRequestActivityRepository meetingRequestActivityRepository,
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator,
      ILogger<ConnectMeetingRequestCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingRequestActivityRepository = meetingRequestActivityRepository;
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
      _logger = logger;
    }

    public override async Task<Unit> Handle(ConnectMeetingRequestCommand request)
    {
      await ValidateData(request);
      var user = await _usersRepository.FindOneWithDetails(request.UserId);
      var connectingMeetingRequest = await _meetingRequestsRepository.FindOneForUserWithUserDetails(request.MeetingRequestId, request.UserId);

      if (connectingMeetingRequest != null)
      {
        await ConnectUsersToMeeting(user, connectingMeetingRequest);
      }
      else
      {
        throw new NotFoundException(nameof(MeetingRequest), request.MeetingRequestId);
      }

      return Unit.Value;
    }

    private async Task ValidateData(ConnectMeetingRequestCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var requestExists = await _meetingRequestsRepository.ExistsOne(request.MeetingRequestId);

      if (!requestExists)
      {
        throw new NotFoundException(nameof(MeetingRequest), request.MeetingRequestId);
      }

      var userRequestExists = await _meetingRequestsRepository.ExistsOneFoundByUserId(request.UserId);

      if (userRequestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      var userMeetingExists = await _groupUsersRepository.ExistsOneByUserId(request.UserId);

      if (userMeetingExists)
      {
        throw new ConflictException(
          $"Entity {nameof(Meeting)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }
    }

    private async Task ConnectUsersToMeeting(User user, MeetingRequest connectingMeetingRequest)
    {
      using (var transaction = _groupUsersRepository.BeginTransaction())
      {
        try
        {
          await AbortSearchingRequest(user);
          var request = await CreateNewMeetingRequest(user, connectingMeetingRequest);
          var meeting = await CreateNewMeeting(request, connectingMeetingRequest);
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
    }

    private async Task AbortSearchingRequest(User user)
    {
      var request = await _meetingRequestsRepository.FindOneSearchingByUserId(user.Id);

      if (request != null)
      {
        request.Abort();
        await _meetingRequestsRepository.Update(request);
      }
    }

    private async Task<MeetingRequest> CreateNewMeetingRequest(User user, MeetingRequest connectingMeetingRequest)
    {
      var meetingRequest = new MeetingRequest(
        connectingMeetingRequest.MinDate,
        connectingMeetingRequest.MaxDate,
        connectingMeetingRequest.User.Profile.Birthday.GetAge(),
        connectingMeetingRequest.User.Profile.Birthday.GetAge() + 5,
        connectingMeetingRequest.Latitude,
        connectingMeetingRequest.Longitude,
        user.Id);

      await _meetingRequestsRepository.Add(meetingRequest);

      connectingMeetingRequest.Activities.ForEach(x =>
      {
        meetingRequest.Activities.Add(new MeetingRequestActivity(meetingRequest.Id, x.ActivityId));
      });

      await _meetingRequestActivityRepository.AddRange(meetingRequest.Activities);

      return meetingRequest;
    }

    private async Task<Meeting> CreateNewMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var group = new Group();
      await _groupsRepository.Add(group);

      var meeting = new Meeting(
        request1.FindRequiredCommonDate(request2),
        request1.Latitude,
        request1.Longitude,
        group.Id,
        request1.FindRequiredCommonActivityId(request2));

      await _meetingsRepository.Add(meeting);

      var groupUsers = new[]
      {
        new GroupUser(group.Id, request1.UserId, request1.Id),
        new GroupUser(group.Id, request2.UserId, request2.Id),
      };

      await _groupUsersRepository.AddRange(groupUsers);

      request1.MarkAsFound();
      request2.MarkAsFound();
      await _meetingRequestsRepository.Update(request1);
      await _meetingRequestsRepository.Update(request2);

      return meeting;
    }
  }
}

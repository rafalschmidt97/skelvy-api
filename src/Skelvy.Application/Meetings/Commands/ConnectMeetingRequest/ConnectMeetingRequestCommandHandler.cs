using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Notifications;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
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
    private readonly IMeetingRequestDrinkTypesRepository _meetingRequestDrinkTypesRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly INotificationsService _notifications;
    private readonly ILogger<ConnectMeetingRequestCommandHandler> _logger;

    public ConnectMeetingRequestCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingRequestDrinkTypesRepository meetingRequestDrinkTypesRepository,
      IMeetingUsersRepository meetingUsersRepository,
      INotificationsService notifications,
      ILogger<ConnectMeetingRequestCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingRequestDrinkTypesRepository = meetingRequestDrinkTypesRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _notifications = notifications;
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

      var userRequestExists = await _meetingRequestsRepository.ExistsOneByUserId(request.UserId);

      if (userRequestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      var userMeetingExists = await _meetingUsersRepository.ExistsOneByUserId(request.UserId);

      if (userMeetingExists)
      {
        throw new ConflictException(
          $"Entity {nameof(Meeting)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }
    }

    private async Task ConnectUsersToMeeting(User user, MeetingRequest connectingMeetingRequest)
    {
      using (var transaction = _meetingUsersRepository.BeginTransaction())
      {
        try
        {
          var request = await CreateNewMeetingRequest(user, connectingMeetingRequest);
          await CreateNewMeeting(request, connectingMeetingRequest);
          transaction.Commit();
          await BroadcastUserFoundMeeting(request, connectingMeetingRequest);
        }
        catch (Exception exception)
        {
          _logger.LogError(
            $"{nameof(ConnectMeetingRequestCommand)} Exception while CreateNewMeetingRequest/CreateNewMeeting for " +
            $"User(Id={user.Id}): {exception.Message}");
        }
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

      connectingMeetingRequest.DrinkTypes.ForEach(x =>
      {
        meetingRequest.DrinkTypes.Add(new MeetingRequestDrinkType(meetingRequest.Id, x.DrinkTypeId));
      });

      await _meetingRequestDrinkTypesRepository.AddRange(meetingRequest.DrinkTypes);

      return meetingRequest;
    }

    private async Task CreateNewMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var meeting = new Meeting(
        request1.FindRequiredCommonDate(request2),
        request1.Latitude,
        request1.Longitude,
        request1.FindRequiredCommonDrinkTypeId(request2));

      await _meetingsRepository.Add(meeting);

      var meetingUsers = new[]
      {
        new MeetingUser(meeting.Id, request1.UserId, request1.Id),
        new MeetingUser(meeting.Id, request2.UserId, request2.Id),
      };

      await _meetingUsersRepository.AddRange(meetingUsers);

      request1.MarkAsFound();
      request2.MarkAsFound();
      await _meetingRequestsRepository.Update(request1);
      await _meetingRequestsRepository.Update(request2);
    }

    private async Task BroadcastUserFoundMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var usersId = new List<int> { request1.UserId, request2.UserId };
      await _notifications.BroadcastUserFoundMeeting(new UserFoundMeetingAction(), usersId);
    }
  }
}

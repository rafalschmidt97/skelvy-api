using System;
using System.Linq;
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

namespace Skelvy.Application.Meetings.Commands.JoinMeeting
{
  public class JoinMeetingCommandHandler : CommandHandler<JoinMeetingCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingRequestDrinkTypesRepository _meetingRequestDrinkTypesRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly INotificationsService _notifications;
    private readonly ILogger<JoinMeetingCommandHandler> _logger;

    public JoinMeetingCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingRequestDrinkTypesRepository meetingRequestDrinkTypesRepository,
      IMeetingUsersRepository meetingUsersRepository,
      INotificationsService notifications,
      ILogger<JoinMeetingCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingRequestDrinkTypesRepository = meetingRequestDrinkTypesRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _notifications = notifications;
      _logger = logger;
    }

    public override async Task<Unit> Handle(JoinMeetingCommand request)
    {
      await ValidateData(request);
      var user = await _usersRepository.FindOneWithDetails(request.UserId);
      var meeting = await _meetingsRepository.FindOneForUserWithUsersDetails(request.MeetingId, request.UserId);

      if (meeting != null)
      {
        await JoinUserToMeeting(user, meeting);
      }
      else
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      return Unit.Value;
    }

    private async Task ValidateData(JoinMeetingCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var meetingExists = await _meetingsRepository.ExistsOne(request.MeetingId);

      if (!meetingExists)
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      var requestExists = await _meetingRequestsRepository.ExistsOneFoundByUserId(request.UserId);

      if (requestExists)
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

    private async Task JoinUserToMeeting(User user, Meeting meeting)
    {
      using (var transaction = _meetingUsersRepository.BeginTransaction())
      {
        try
        {
          await AbortSearchingRequest(user);
          var request = await CreateNewMeetingRequest(user, meeting);
          var meetingUser = await AddUserToMeeting(request, meeting);
          transaction.Commit();
          await BroadcastUserJoinedMeeting(meetingUser);
        }
        catch (Exception exception)
        {
          _logger.LogError(
            $"{nameof(JoinMeetingCommand)} Exception while CreateNewMeetingRequest/AddUserToMeeting for " +
            $"Meeting(Id={meeting.Id}) User(Id={user.Id}): {exception.Message}");
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

    private async Task<MeetingRequest> CreateNewMeetingRequest(User user, Meeting meeting)
    {
      var minBirthday = meeting.Users.Select(x => x.User.Profile.Birthday).Min();
      var maxBirthday = meeting.Users.Select(x => x.User.Profile.Birthday).Max();

      var meetingRequest = new MeetingRequest(
        meeting.Date.AddDays(-1),
        meeting.Date.AddDays(1),
        maxBirthday.GetAge(),
        minBirthday.GetAge() + 5,
        meeting.Latitude,
        meeting.Longitude,
        user.Id);

      await _meetingRequestsRepository.Add(meetingRequest);
      meetingRequest.DrinkTypes.Add(new MeetingRequestDrinkType(meetingRequest.Id, meeting.DrinkTypeId));
      await _meetingRequestDrinkTypesRepository.AddRange(meetingRequest.DrinkTypes);

      return meetingRequest;
    }

    private async Task<MeetingUser> AddUserToMeeting(MeetingRequest newRequest, Meeting meeting)
    {
      var meetingUser = new MeetingUser(meeting.Id, newRequest.UserId, newRequest.Id);
      await _meetingUsersRepository.Add(meetingUser);
      newRequest.MarkAsFound();
      await _meetingRequestsRepository.Update(newRequest);
      return meetingUser;
    }

    private async Task BroadcastUserJoinedMeeting(MeetingUser meetingUser)
    {
      var meetingUsers = await _meetingUsersRepository.FindAllByMeetingId(meetingUser.MeetingId);

      var meetingUsersId = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserJoinedMeeting(new UserJoinedMeetingAction(), meetingUsersId);
    }
  }
}

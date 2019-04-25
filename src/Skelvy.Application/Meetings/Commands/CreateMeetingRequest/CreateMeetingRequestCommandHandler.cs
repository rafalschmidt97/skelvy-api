using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Drinks.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Notifications;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Extensions;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommandHandler : CommandHandler<CreateMeetingRequestCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IDrinksRepository _drinksRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly INotificationsService _notifications;

    public CreateMeetingRequestCommandHandler(
      IUsersRepository usersRepository,
      IDrinksRepository drinksRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingUsersRepository meetingUsersRepository,
      INotificationsService notifications)
    {
      _usersRepository = usersRepository;
      _drinksRepository = drinksRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _notifications = notifications;
    }

    public override async Task<Unit> Handle(CreateMeetingRequestCommand request)
    {
      await ValidateData(request);
      var newRequest = await CreateNewMeetingRequest(request);
      var user = await _usersRepository.FindOneWithDetails(request.UserId);
      var meeting = await _meetingsRepository.FindOneMatchingUserRequest(user, newRequest);

      if (meeting != null)
      {
        await AddUserToMeeting(newRequest, meeting);
      }
      else
      {
        await MatchExistingMeetingRequests(newRequest, user);
      }

      return Unit.Value;
    }

    private async Task ValidateData(CreateMeetingRequestCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var drinks = await _drinksRepository.FindAll();
      var filteredDrinks = drinks.Where(x => request.Drinks.Any(y => y.Id == x.Id)).ToList();

      if (filteredDrinks.Count != request.Drinks.Count)
      {
        throw new NotFoundException(nameof(Drink), request.Drinks);
      }

      var requestExists = await _meetingRequestsRepository.ExistsOneByUserId(request.UserId);

      if (requestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      var meetingExists = await _meetingUsersRepository.ExistsOneByUserId(request.UserId);

      if (meetingExists)
      {
        throw new ConflictException(
          $"Entity {nameof(Meeting)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }
    }

    private async Task<MeetingRequest> CreateNewMeetingRequest(CreateMeetingRequestCommand request)
    {
      var meetingRequest = new MeetingRequest(
        request.MinDate,
        request.MaxDate,
        request.MinAge,
        request.MaxAge,
        request.Latitude,
        request.Longitude,
        request.UserId);

      _meetingRequestsRepository.Context.MeetingRequests.Add(meetingRequest);

      var meetingRequestDrinks = PrepareDrinks(request.Drinks, meetingRequest);
      _meetingRequestsRepository.Context.MeetingRequestDrinks.AddRange(meetingRequestDrinks);

      await _meetingRequestsRepository.Context.SaveChangesAsync();
      return meetingRequest;
    }

    private async Task AddUserToMeeting(MeetingRequest newRequest, Meeting meeting)
    {
      var meetingUser = new MeetingUser(meeting.Id, newRequest.UserId, newRequest.Id);
      _meetingUsersRepository.Context.MeetingUsers.Add(meetingUser);
      newRequest.MarkAsFound();

      await _meetingUsersRepository.Context.SaveChangesAsync();
      await BroadcastUserJoinedMeeting(meetingUser);
    }

    private async Task MatchExistingMeetingRequests(MeetingRequest newRequest, User user)
    {
      var existingRequest = await _meetingRequestsRepository.FindOneMatchingUserRequest(user, newRequest);

      if (existingRequest != null)
      {
        await CreateNewMeeting(newRequest, existingRequest);
      }
    }

    private async Task CreateNewMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var meeting = new Meeting(
        request1.FindCommonDate(request2),
        request1.Latitude,
        request1.Longitude,
        request1.FindCommonDrinkId(request2));

      _meetingsRepository.Context.Meetings.Add(meeting);

      var meetingUsers = new[]
      {
        new MeetingUser(meeting.Id, request1.UserId, request1.Id),
        new MeetingUser(meeting.Id, request2.UserId, request2.Id),
      };

      _meetingUsersRepository.Context.MeetingUsers.AddRange(meetingUsers);

      request1.MarkAsFound();
      request2.MarkAsFound();

      await _meetingsRepository.Context.SaveChangesAsync();
      await BroadcastUserFoundMeeting(request1, request2);
    }

    private async Task BroadcastUserFoundMeeting(MeetingRequest request1, MeetingRequest request2)
    {
      var userIds = new List<int> { request1.UserId, request2.UserId };
      await _notifications.BroadcastUserFoundMeeting(userIds);
    }

    private async Task BroadcastUserJoinedMeeting(MeetingUser meetingUser)
    {
      var meetingUsers = await _meetingUsersRepository.FindAllByMeetingId(meetingUser.MeetingId);

      var meetingUserIds = meetingUsers.Where(x => x.UserId != meetingUser.UserId).Select(x => x.UserId).ToList();
      await _notifications.BroadcastUserJoinedMeeting(meetingUser, meetingUserIds);
    }

    private static IEnumerable<MeetingRequestDrink> PrepareDrinks(
      IEnumerable<CreateMeetingRequestDrink> requestDrinks,
      MeetingRequest request)
    {
      return requestDrinks.Select(requestDrink => new MeetingRequestDrink(request.Id, requestDrink.Id));
    }
  }
}

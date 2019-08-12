using System;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.UserJoinedGroup;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
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
    private readonly IMeetingRequestActivityRepository _meetingRequestActivityRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<JoinMeetingCommandHandler> _logger;

    public JoinMeetingCommandHandler(
      IUsersRepository usersRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingRequestActivityRepository meetingRequestActivityRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator,
      ILogger<JoinMeetingCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingRequestActivityRepository = meetingRequestActivityRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
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
    }

    private async Task JoinUserToMeeting(User user, Meeting meeting)
    {
      using (var transaction = _groupUsersRepository.BeginTransaction())
      {
        try
        {
          var request = await CreateNewMeetingRequest(user, meeting);
          var groupUser = await AddUserToMeeting(request, meeting);
          transaction.Commit();
          await _mediator.Publish(new UserJoinedGroupEvent(groupUser.UserId, groupUser.GroupId));
        }
        catch (Exception exception)
        {
          _logger.LogError(
            $"{nameof(JoinMeetingCommand)} Exception while CreateNewMeetingRequest/AddUserToMeeting for " +
            $"Meeting(Id={meeting.Id}) User(Id={user.Id}): {exception.Message}");
        }
      }
    }

    private async Task<MeetingRequest> CreateNewMeetingRequest(User user, Meeting meeting)
    {
      var minBirthday = meeting.Group.Users.Select(x => x.User.Profile.Birthday).Min();
      var maxBirthday = meeting.Group.Users.Select(x => x.User.Profile.Birthday).Max();

      var meetingRequest = new MeetingRequest(
        meeting.Date.AddDays(-1),
        meeting.Date.AddDays(1),
        maxBirthday.GetAge(),
        minBirthday.GetAge() + 5,
        meeting.Latitude,
        meeting.Longitude,
        user.Id);
      meetingRequest.MarkAsFound();

      await _meetingRequestsRepository.Add(meetingRequest);
      meetingRequest.Activities.Add(new MeetingRequestActivity(meetingRequest.Id, meeting.ActivityId));
      await _meetingRequestActivityRepository.AddRange(meetingRequest.Activities);

      return meetingRequest;
    }

    private async Task<GroupUser> AddUserToMeeting(MeetingRequest newRequest, Meeting meeting)
    {
      var groupUser = new GroupUser(meeting.Id, newRequest.UserId, newRequest.Id);
      await _groupUsersRepository.Add(groupUser);
      return groupUser;
    }
  }
}

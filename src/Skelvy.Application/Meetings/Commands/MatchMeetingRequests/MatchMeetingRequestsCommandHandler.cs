using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Events.UsersConnectedToMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Extensions;

namespace Skelvy.Application.Meetings.Commands.MatchMeetingRequests
{
  public class MatchMeetingRequestsCommandHandler : CommandHandler<MatchMeetingRequestsCommand>
  {
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupsRepository _groupsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<MatchMeetingRequestsCommandHandler> _logger;

    public MatchMeetingRequestsCommandHandler(
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingsRepository meetingsRepository,
      IGroupsRepository groupsRepository,
      IGroupUsersRepository groupUsersRepository,
      IMediator mediator,
      ILogger<MatchMeetingRequestsCommandHandler> logger)
    {
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingsRepository = meetingsRepository;
      _groupsRepository = groupsRepository;
      _groupUsersRepository = groupUsersRepository;
      _mediator = mediator;
      _logger = logger;
    }

    public override async Task<Unit> Handle(MatchMeetingRequestsCommand request)
    {
      var requests = await _meetingRequestsRepository.FindAllSearchingWithUsersDetailsAndActivities();

      foreach (var meetingRequest in requests)
      {
        var existingRequest = await _meetingRequestsRepository.FindOneMatchingUserRequest(meetingRequest.User, meetingRequest);

        if (existingRequest != null)
        {
          await CreateNewMeeting(meetingRequest, existingRequest);
        }
      }

      return Unit.Value;
    }

    private async Task CreateNewMeeting(MeetingRequest request1, MeetingRequest request2) // Same logic as CreateMeetingRequest
    {
      using (var transaction = _meetingRequestsRepository.BeginTransaction())
      {
        try
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

          transaction.Commit();
          await _mediator.Publish(new UsersConnectedToMeetingEvent(request1.UserId, request2.UserId, meeting.Id));
        }
        catch (Exception exception)
        {
          if (exception is DbUpdateConcurrencyException)
          {
            _logger.LogError(
              $"{nameof(MatchMeetingRequestsCommand)} Concurrency Exception for " +
              $"Requests(Id={request1.Id}, Id={request2.Id})");
          }
          else
          {
            _logger.LogError(
              $"{nameof(MatchMeetingRequestsCommand)} Exception for " +
              $"Requests(Id={request1.Id}, Id={request2.Id}): {exception.Message}");
          }
        }
      }
    }
  }
}

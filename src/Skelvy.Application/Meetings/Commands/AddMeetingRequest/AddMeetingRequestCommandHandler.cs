using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Activities.Infrastructure.Repositories;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.AddMeetingRequest
{
  public class AddMeetingRequestCommandHandler : CommandHandler<AddMeetingRequestCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IActivitiesRepository _activitiesRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingRequestActivityRepository _meetingRequestActivityRepository;

    public AddMeetingRequestCommandHandler(
      IUsersRepository usersRepository,
      IActivitiesRepository activitiesRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingRequestActivityRepository meetingRequestActivityRepository)
    {
      _usersRepository = usersRepository;
      _activitiesRepository = activitiesRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingRequestActivityRepository = meetingRequestActivityRepository;
    }

    public override async Task<Unit> Handle(AddMeetingRequestCommand request)
    {
      await ValidateData(request);

      using (var transaction = _meetingRequestsRepository.BeginTransaction())
      {
        var meetingRequest = new MeetingRequest(
          request.MinDate,
          request.MaxDate,
          request.MinAge,
          request.MaxAge,
          request.Latitude,
          request.Longitude,
          request.UserId);

        await _meetingRequestsRepository.Add(meetingRequest);
        meetingRequest.Activities = new List<MeetingRequestActivity>();
        PrepareActivities(request.Activities, meetingRequest).ForEach(x => meetingRequest.Activities.Add(x));
        await _meetingRequestActivityRepository.AddRange(meetingRequest.Activities);
        transaction.Commit();
      }

      return Unit.Value;
    }

    private async Task ValidateData(AddMeetingRequestCommand request)
    {
      var userExists = await _usersRepository.ExistsOne(request.UserId);

      if (!userExists)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var activities = await _activitiesRepository.FindAll();
      var filteredActivities = activities.Where(x => request.Activities.Any(y => y.Id == x.Id)).ToList();

      if (filteredActivities.Count != request.Activities.Count)
      {
        throw new NotFoundException(nameof(Activity), request.Activities);
      }

      var meetingRequestCount = await _meetingRequestsRepository.CountSearchingByUserId(request.UserId);

      if (meetingRequestCount >= 3)
      {
        throw new ConflictException($"{nameof(User)}(Id = {request.UserId}) has already {meetingRequestCount} {nameof(MeetingRequest)}s. " +
                                             $"You can have up to 3 {nameof(MeetingRequest)} simultaneity. Remove one first.");
      }
    }

    private static IEnumerable<MeetingRequestActivity> PrepareActivities(
      IEnumerable<AddMeetingRequestActivity> requestActivities,
      MeetingRequest request)
    {
      return requestActivities.Select(activity => new MeetingRequestActivity(request.Id, activity.Id));
    }
  }
}

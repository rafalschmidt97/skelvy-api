using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Activities.Infrastructure.Repositories;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.UpdateMeetingRequest
{
  public class UpdateMeetingRequestCommandHandler : CommandHandler<UpdateMeetingRequestCommand>
  {
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingRequestActivityRepository _meetingRequestActivityRepository;
    private readonly IActivitiesRepository _activitiesRepository;

    public UpdateMeetingRequestCommandHandler(
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingRequestActivityRepository meetingRequestActivityRepository,
      IActivitiesRepository activitiesRepository)
    {
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingRequestActivityRepository = meetingRequestActivityRepository;
      _activitiesRepository = activitiesRepository;
    }

    public override async Task<Unit> Handle(UpdateMeetingRequestCommand request)
    {
      var meetingRequest = await ValidateData(request);

      using (var transaction = _meetingRequestsRepository.BeginTransaction())
      {
        meetingRequest.Update(request.MinDate, request.MaxDate, request.MinAge, request.MaxAge, request.Latitude, request.Longitude, request.Description);
        await _meetingRequestsRepository.Update(meetingRequest);
        await UpdateActivities(meetingRequest, request.Activities);
        transaction.Commit();
      }

      return Unit.Value;
    }

    private async Task<MeetingRequest> ValidateData(UpdateMeetingRequestCommand request)
    {
      var meetingRequest = await _meetingRequestsRepository.FindOneSearchingByRequestId(request.RequestId);

      if (meetingRequest == null)
      {
        throw new NotFoundException(nameof(MeetingRequest), request.RequestId);
      }

      if (meetingRequest.UserId != request.UserId)
      {
        throw new ForbiddenException(
          $"{nameof(MeetingRequest)}({request.RequestId}) does not belong to {nameof(User)}({request.UserId}");
      }

      var activities = await _activitiesRepository.FindAll();
      var filteredActivities = activities.Where(x => request.Activities.Any(y => y.Id == x.Id)).ToList();

      if (filteredActivities.Count != request.Activities.Count)
      {
        throw new NotFoundException(nameof(Activity), request.Activities);
      }

      return meetingRequest;
    }

    private async Task UpdateActivities(MeetingRequest meetingRequest, IList<UpdateMeetingRequestActivity> activities)
    {
      var oldActivities = await _meetingRequestActivityRepository.FindAllByRequestId(meetingRequest.Id);

      if (oldActivities.Any())
      {
        await _meetingRequestActivityRepository.RemoveRange(oldActivities);
      }

      var newActivities = activities.Select((activity, index) => new MeetingRequestActivity(meetingRequest.Id, activity.Id)).ToList();
      await _meetingRequestActivityRepository.AddRange(newActivities);
    }
  }
}

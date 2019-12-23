using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Activities.Infrastructure.Repositories;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Groups.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Events.MeetingUpdated;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Meetings.Commands.UpdateMeeting
{
  public class UpdateMeetingCommandHandler : CommandHandler<UpdateMeetingCommand>
  {
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IActivitiesRepository _activitiesRepository;
    private readonly IMediator _mediator;

    public UpdateMeetingCommandHandler(
      IMeetingsRepository meetingsRepository,
      IGroupUsersRepository groupUsersRepository,
      IActivitiesRepository activitiesRepository,
      IMediator mediator)
    {
      _meetingsRepository = meetingsRepository;
      _groupUsersRepository = groupUsersRepository;
      _activitiesRepository = activitiesRepository;
      _mediator = mediator;
    }

    public override async Task<Unit> Handle(UpdateMeetingCommand request)
    {
      var meeting = await ValidateData(request);

      meeting.Update(request.Date, request.Latitude, request.Longitude, request.Size, request.Description, request.IsHidden, request.ActivityId);
      await _meetingsRepository.Update(meeting);
      await _mediator.Publish(new MeetingUpdatedEvent(request.UserId, meeting.Id, meeting.GroupId));

      return Unit.Value;
    }

    private async Task<Meeting> ValidateData(UpdateMeetingCommand request)
    {
      var meeting = await _meetingsRepository.FindOne(request.MeetingId);

      if (meeting == null)
      {
        throw new NotFoundException(nameof(Meeting), request.MeetingId);
      }

      var existsUser = await _groupUsersRepository.ExistsOneByUserIdAndGroupId(request.UserId, meeting.GroupId);

      if (!existsUser)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var existsActivity = await _activitiesRepository.ExistsOne(request.ActivityId);

      if (!existsActivity)
      {
        throw new NotFoundException(nameof(Activity), request.ActivityId);
      }

      return meeting;
    }
  }
}

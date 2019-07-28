using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Drinks.Infrastructure.Repositories;
using Skelvy.Application.Meetings.Events.UserFoundMeeting;
using Skelvy.Application.Meetings.Events.UserJoinedMeeting;
using Skelvy.Application.Meetings.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Common.Extensions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Extensions;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommandHandler : CommandHandler<CreateMeetingRequestCommand>
  {
    private readonly IUsersRepository _usersRepository;
    private readonly IDrinkTypesRepository _drinkTypesRepository;
    private readonly IMeetingsRepository _meetingsRepository;
    private readonly IMeetingRequestsRepository _meetingRequestsRepository;
    private readonly IMeetingRequestDrinkTypesRepository _meetingRequestDrinkTypesRepository;
    private readonly IMeetingUsersRepository _meetingUsersRepository;
    private readonly IMediator _mediator;
    private readonly ILogger<CreateMeetingRequestCommandHandler> _logger;

    public CreateMeetingRequestCommandHandler(
      IUsersRepository usersRepository,
      IDrinkTypesRepository drinkTypesRepository,
      IMeetingsRepository meetingsRepository,
      IMeetingRequestsRepository meetingRequestsRepository,
      IMeetingRequestDrinkTypesRepository meetingRequestDrinkTypesRepository,
      IMeetingUsersRepository meetingUsersRepository,
      IMediator mediator,
      ILogger<CreateMeetingRequestCommandHandler> logger)
    {
      _usersRepository = usersRepository;
      _drinkTypesRepository = drinkTypesRepository;
      _meetingsRepository = meetingsRepository;
      _meetingRequestsRepository = meetingRequestsRepository;
      _meetingRequestDrinkTypesRepository = meetingRequestDrinkTypesRepository;
      _meetingUsersRepository = meetingUsersRepository;
      _mediator = mediator;
      _logger = logger;
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
        var existingRequest = await _meetingRequestsRepository.FindOneMatchingUserRequest(user, newRequest);

        if (existingRequest != null)
        {
          await CreateNewMeeting(newRequest, existingRequest);
        }
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

      var drinkTypes = await _drinkTypesRepository.FindAll();
      var filteredDrinkTypes = drinkTypes.Where(x => request.DrinkTypes.Any(y => y.Id == x.Id)).ToList();

      if (filteredDrinkTypes.Count != request.DrinkTypes.Count)
      {
        throw new NotFoundException(nameof(DrinkType), request.DrinkTypes);
      }

      var requestExists = await _meetingRequestsRepository.ExistsOneByUserId(request.UserId);

      if (requestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }
    }

    private async Task<MeetingRequest> CreateNewMeetingRequest(CreateMeetingRequestCommand request)
    {
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
        PrepareDrinkTypes(request.DrinkTypes, meetingRequest).ForEach(x => meetingRequest.DrinkTypes.Add(x));
        await _meetingRequestDrinkTypesRepository.AddRange(meetingRequest.DrinkTypes);
        transaction.Commit();

        return meetingRequest;
      }
    }

    private async Task AddUserToMeeting(MeetingRequest newRequest, Meeting meeting)
    {
      using (var transaction = _meetingUsersRepository.BeginTransaction())
      {
        try
        {
          var meetingUser = new MeetingUser(meeting.Id, newRequest.UserId, newRequest.Id);
          await _meetingUsersRepository.Add(meetingUser);
          newRequest.MarkAsFound();
          await _meetingRequestsRepository.Update(newRequest);

          transaction.Commit();
          await _mediator.Publish(new UserJoinedMeetingEvent(meetingUser.UserId, meetingUser.MeetingId));
        }
        catch (Exception exception)
        {
          _logger.LogError(
            $"{nameof(CreateMeetingRequestCommand)} Exception while AddUserToMeeting for " +
            $"Meeting(Id={meeting.Id}) Request(Id={newRequest.Id}): {exception.Message}");
        }
      }
    }

    private async Task CreateNewMeeting(MeetingRequest newRequest, MeetingRequest existingRequest)
    {
      using (var transaction = _meetingsRepository.BeginTransaction())
      {
        try
        {
          var meeting = new Meeting(
            newRequest.FindRequiredCommonDate(existingRequest),
            newRequest.Latitude,
            newRequest.Longitude,
            newRequest.FindRequiredCommonDrinkTypeId(existingRequest));

          await _meetingsRepository.Add(meeting);

          var meetingUsers = new[]
          {
            new MeetingUser(meeting.Id, newRequest.UserId, newRequest.Id),
            new MeetingUser(meeting.Id, existingRequest.UserId, existingRequest.Id),
          };

          await _meetingUsersRepository.AddRange(meetingUsers);

          newRequest.MarkAsFound();
          existingRequest.MarkAsFound();
          await _meetingRequestsRepository.Update(newRequest);
          await _meetingRequestsRepository.Update(existingRequest);

          transaction.Commit();
          await _mediator.Publish(new UserFoundMeetingEvent(existingRequest.UserId, meeting.Id));
        }
        catch (Exception exception)
        {
          if (exception is DbUpdateConcurrencyException)
          {
            _logger.LogError(
              $"{nameof(CreateMeetingRequestCommand)} Concurrency Exception for while " +
              $"CreateNewMeeting Requests(Id={newRequest.Id}, Id={existingRequest.Id})");
          }
          else
          {
            _logger.LogError(
              $"{nameof(CreateMeetingRequestCommand)} Exception for while CreateNewMeeting " +
              $"Requests({newRequest.Id}, {existingRequest.Id}): {exception.Message}");
          }
        }
      }
    }

    private static IEnumerable<MeetingRequestDrinkType> PrepareDrinkTypes(
      IEnumerable<CreateMeetingRequestDrinkType> requestDrinkTypes,
      MeetingRequest request)
    {
      return requestDrinkTypes.Select(requestDrink => new MeetingRequestDrinkType(request.Id, requestDrink.Id));
    }
  }
}

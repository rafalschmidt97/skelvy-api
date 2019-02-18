using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommandHandler : IRequestHandler<CreateMeetingRequestCommand>
  {
    private readonly SkelvyContext _context;

    public CreateMeetingRequestCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(CreateMeetingRequestCommand request, CancellationToken cancellationToken)
    {
      // check if data is valid (user and drinks exist)
      var requestExists = _context.MeetingRequests.Any(x => x.UserId == request.UserId);

      if (requestExists)
      {
        throw new ConflictException(
          $"Entity {nameof(MeetingRequest)}({nameof(request.UserId)}={request.UserId}) already exists.");
      }

      var user = await _context.Users
        .FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

      if (user == null)
      {
        throw new NotFoundException(nameof(User), request.UserId);
      }

      var drinks = await _context.Drinks.Where(x => request.Drinks.Any(y => y.Id == x.Id))
        .ToListAsync(cancellationToken);

      if (drinks.Count != request.Drinks.Count)
      {
        throw new NotFoundException(nameof(Drink), request.Drinks);
      }

      // TODO: check if meetings or other requests cover needs (then add/create meeting and remove other requests)

      // if meeting not found add new request
      var meetingRequest = new MeetingRequest
      {
        MinDate = request.MinDate,
        MaxDate = request.MaxDate,
        MinAge = request.MinAge,
        MaxAge = request.MaxAge,
        Latitude = request.Latitude,
        Longitude = request.Longitude,
        UserId = user.Id
      };
      _context.MeetingRequests.Add(meetingRequest);

      var meetingRequestDrinks = PrepareDrinks(request.Drinks, meetingRequest);
      _context.MeetingRequestDrinks.AddRange(meetingRequestDrinks);

      await _context.SaveChangesAsync(cancellationToken);

      return Unit.Value;
    }

    private static IEnumerable<MeetingRequestDrink> PrepareDrinks(
      IEnumerable<CreateMeetingRequestDrink> requestDrinks,
      MeetingRequest request)
    {
      return requestDrinks.Select(requestDrink => new MeetingRequestDrink
      {
        MeetingRequestId = request.Id,
        DrinkId = requestDrink.Id
      });
    }
  }
}

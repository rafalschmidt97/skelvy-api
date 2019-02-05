using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Exceptions.Extra;
using Skelvy.Application.Core.Infrastructure;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.CreateUser
{
  public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
  {
    private readonly SkelvyContext _context;
    private readonly INotificationService _notificationService;

    public CreateUserCommandHandler(SkelvyContext context, INotificationService notificationService)
    {
      _context = context;
      _notificationService = notificationService;
    }

    public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
      await _notificationService.Send("Hello World");

      var entity = new User
      {
        Email = request.Email,
        Name = request.Name
      };

      var user = await _context.Users.FirstOrDefaultAsync(
        value => value.Email == request.Email, cancellationToken);

      if (user != null)
      {
        throw new AlreadyExistsException(nameof(User), request.Email);
      }

      _context.Users.Add(entity);
      await _context.SaveChangesAsync(cancellationToken);

      return entity.Id;
    }
  }
}

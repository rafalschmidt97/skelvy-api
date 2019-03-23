using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.UpdateUserProfile
{
  public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand>
  {
    private readonly SkelvyContext _context;

    public UpdateUserProfileCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public async Task<Unit> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
    {
      var profile = await _context.UserProfiles
        .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

      if (profile == null)
      {
        throw new NotFoundException(nameof(UserProfile), request.UserId);
      }

      profile.Name = request.Name.Trim();
      profile.Birthday = request.Birthday;
      profile.Gender = request.Gender;

      if (request.Description != null)
      {
        profile.Description = request.Description.Trim();
      }

      // Remove old photos
      var oldPhotos = await _context.UserProfilePhotos.Where(x => x.ProfileId == profile.Id)
        .ToListAsync(cancellationToken);
      _context.UserProfilePhotos.RemoveRange(oldPhotos);

      // Add new photos
      var newPhotos = PreparePhotos(request.Photos, profile);
      _context.UserProfilePhotos.AddRange(newPhotos);

      await _context.SaveChangesAsync(cancellationToken);
      return Unit.Value;
    }

    private static IEnumerable<UserProfilePhoto> PreparePhotos(
      IEnumerable<UpdateUserProfilePhotos> photos,
      UserProfile profile)
    {
      return photos.Select(photo => new UserProfilePhoto
      {
        Url = photo.Url,
        ProfileId = profile.Id
      });
    }
  }
}

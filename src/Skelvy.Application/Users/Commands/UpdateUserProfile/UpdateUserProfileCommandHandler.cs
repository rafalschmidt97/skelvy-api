using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Bus;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Persistence;

namespace Skelvy.Application.Users.Commands.UpdateUserProfile
{
  public class UpdateUserProfileCommandHandler : CommandHandler<UpdateUserProfileCommand>
  {
    private readonly SkelvyContext _context;

    public UpdateUserProfileCommandHandler(SkelvyContext context)
    {
      _context = context;
    }

    public override async Task<Unit> Handle(UpdateUserProfileCommand request)
    {
      var profile = await _context.UserProfiles
        .FirstOrDefaultAsync(x => x.UserId == request.UserId);

      if (profile == null)
      {
        throw new NotFoundException(nameof(UserProfile), request.UserId);
      }

      profile.Update(request.Name, request.Birthday, request.Gender, request.Description);
      await UpdatePhotos(profile, request.Photos);

      await _context.SaveChangesAsync();
      return Unit.Value;
    }

    private async Task UpdatePhotos(UserProfile profile, IEnumerable<UpdateUserProfilePhotos> photos)
    {
      // Remove old photos
      var oldPhotos = await _context.UserProfilePhotos.Where(x => x.ProfileId == profile.Id).ToListAsync();
      _context.UserProfilePhotos.RemoveRange(oldPhotos);

      // Add new photos
      var newPhotos = PreparePhotos(photos, profile);
      _context.UserProfilePhotos.AddRange(newPhotos);
    }

    private static IEnumerable<UserProfilePhoto> PreparePhotos(
      IEnumerable<UpdateUserProfilePhotos> photos,
      UserProfile profile)
    {
      return photos.Select(photo => new UserProfilePhoto(photo.Url, profile.Id));
    }
  }
}

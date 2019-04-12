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

      profile.Name = request.Name.Trim();
      profile.Birthday = request.Birthday;
      profile.Gender = request.Gender;

      if (request.Description != null)
      {
        profile.Description = request.Description.Trim();
      }

      await UpdatePhotos(profile, request.Photos);
      await _context.SaveChangesAsync();
      return Unit.Value;
    }

    private async Task UpdatePhotos(UserProfile profile, IEnumerable<UpdateUserProfilePhotos> photos)
    {
      // Remove old photos
      var oldPhotos = await _context.UserProfilePhotos.Where(x => x.ProfileId == profile.Id)
        .ToListAsync();
      oldPhotos.ForEach(x => { x.Status = UserProfilePhotoStatusTypes.Removed; });

      // Add new photos
      var newPhotos = PreparePhotos(photos, profile);
      _context.UserProfilePhotos.AddRange(newPhotos);
    }

    private static IEnumerable<UserProfilePhoto> PreparePhotos(
      IEnumerable<UpdateUserProfilePhotos> photos,
      UserProfile profile)
    {
      return photos.Select(photo => new UserProfilePhoto
      {
        Url = photo.Url,
        Status = UserProfilePhotoStatusTypes.Active,
        ProfileId = profile.Id,
      });
    }
  }
}

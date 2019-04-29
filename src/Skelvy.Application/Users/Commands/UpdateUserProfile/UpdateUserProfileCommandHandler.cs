using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;

namespace Skelvy.Application.Users.Commands.UpdateUserProfile
{
  public class UpdateUserProfileCommandHandler : CommandHandler<UpdateUserProfileCommand>
  {
    private readonly IUserProfilesRepository _profilesRepository;
    private readonly IUserProfilePhotosRepository _profilePhotosRepository;

    public UpdateUserProfileCommandHandler(IUserProfilesRepository profilesRepository, IUserProfilePhotosRepository profilePhotosRepository)
    {
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
    }

    public override async Task<Unit> Handle(UpdateUserProfileCommand request)
    {
      var profile = await _profilesRepository.FindOneByUserId(request.UserId);

      if (profile == null)
      {
        throw new NotFoundException(nameof(UserProfile), request.UserId);
      }

      using (var transaction = _profilesRepository.BeginTransaction())
      {
        profile.Update(request.Name, request.Birthday, request.Gender, request.Description);
        await _profilesRepository.Update(profile);
        await UpdatePhotos(profile, request.Photos);
        transaction.Commit();
      }

      return Unit.Value;
    }

    private async Task UpdatePhotos(UserProfile profile, IEnumerable<UpdateUserProfilePhotos> photos)
    {
      // Remove old photos
      var oldPhotos = await _profilePhotosRepository.FindAllByProfileId(profile.Id);
      await _profilePhotosRepository.RemoveRange(oldPhotos);

      // Add new photos
      var newPhotos = PreparePhotos(photos, profile);
      await _profilePhotosRepository.AddRange(newPhotos);
    }

    private static IEnumerable<UserProfilePhoto> PreparePhotos(
      IEnumerable<UpdateUserProfilePhotos> photos,
      UserProfile profile)
    {
      return photos.Select(photo => new UserProfilePhoto(photo.Url, profile.Id));
    }
  }
}

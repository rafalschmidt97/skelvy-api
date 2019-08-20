using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Skelvy.Application.Core.Bus;
using Skelvy.Application.Uploads.Infrastructure.Repositories;
using Skelvy.Application.Users.Infrastructure.Repositories;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Commands.UpdateProfile
{
  public class UpdateProfileCommandHandler : CommandHandler<UpdateProfileCommand>
  {
    private readonly IProfilesRepository _profilesRepository;
    private readonly IProfilePhotosRepository _profilePhotosRepository;
    private readonly IAttachmentsRepository _attachmentsRepository;

    public UpdateProfileCommandHandler(
      IProfilesRepository profilesRepository,
      IProfilePhotosRepository profilePhotosRepository,
      IAttachmentsRepository attachmentsRepository)
    {
      _profilesRepository = profilesRepository;
      _profilePhotosRepository = profilePhotosRepository;
      _attachmentsRepository = attachmentsRepository;
    }

    public override async Task<Unit> Handle(UpdateProfileCommand request)
    {
      var profile = await _profilesRepository.FindOneByUserId(request.UserId);

      if (profile == null)
      {
        throw new NotFoundException(nameof(Profile), request.UserId);
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

    private async Task UpdatePhotos(Profile profile, IList<UpdateProfilePhotos> photos)
    {
      var oldPhotos = await _profilePhotosRepository.FindAllWithAttachmentByProfileId(profile.Id);

      if (oldPhotos.Any())
      {
        await _profilePhotosRepository.RemoveRange(oldPhotos);
        var oldAttachments = oldPhotos.Select(x => x.Attachment).ToList();
        await _attachmentsRepository.RemoveRange(oldAttachments);
      }

      var newAttachments = photos.Select((photo, index) => new Attachment(AttachmentType.Image, photo.Url)).ToList();
      await _attachmentsRepository.AddRange(newAttachments);
      var newPhotos = newAttachments.Select((attachment, index) => new ProfilePhoto(attachment.Id, index + 1, profile.Id)).ToList();
      await _profilePhotosRepository.AddRange(newPhotos);
    }
  }
}

using FluentValidation;

namespace Skelvy.Application.Uploads.Commands.UploadPhoto
{
  public class UploadPhotoCommandValidator : AbstractValidator<UploadPhotoCommand>
  {
    public UploadPhotoCommandValidator()
    {
      RuleFor(x => x.Name).NotEmpty();
    }
  }
}

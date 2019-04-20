using System;
using FluentValidation;
using Skelvy.Domain.Enums.Users;

namespace Skelvy.Application.Users.Commands.UpdateUserProfile
{
  public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
  {
    public UpdateUserProfileCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
      RuleFor(x => x.Description).MaximumLength(500);

      RuleFor(x => x.Birthday).NotEmpty()
        .Must(x => x.Date <= DateTimeOffset.UtcNow.AddYears(-18))
        .WithMessage("'Birthday' must show the age of majority.");

      RuleFor(x => x.Gender).NotEmpty().MaximumLength(15)
        .Must(x => x == GenderTypes.Male || x == GenderTypes.Female || x == GenderTypes.Other)
        .WithMessage($"'Gender' must be {GenderTypes.Male} / {GenderTypes.Female} / {GenderTypes.Other}");

      RuleFor(x => x.Photos).NotEmpty()
        .Must(x => x != null && x.Count <= 3)
        .WithMessage("'Photos' must contain fewer than 3 items.");

      RuleForEach(x => x.Photos).SetValidator(new UpdateUserProfilePhotosValidator());
    }
  }

  public class UpdateUserProfilePhotosValidator : AbstractValidator<UpdateUserProfilePhotos>
  {
    public UpdateUserProfilePhotosValidator()
    {
      RuleFor(x => x.Url).NotEmpty().MaximumLength(2048);
    }
  }
}

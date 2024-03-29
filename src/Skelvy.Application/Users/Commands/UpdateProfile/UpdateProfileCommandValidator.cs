using System;
using FluentValidation;
using Skelvy.Domain.Enums;

namespace Skelvy.Application.Users.Commands.UpdateProfile
{
  public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
  {
    public UpdateProfileCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(50)
        .Matches(@"^[\p{L} ]+$")
        .WithMessage("'Name' contains forbidden characters.");

      RuleFor(x => x.Description).MaximumLength(500);

      RuleFor(x => x.Birthday).NotEmpty()
        .Must(x => x.Date <= DateTimeOffset.UtcNow.AddYears(-18))
        .WithMessage("'Birthday' must show the age of majority.");

      RuleFor(x => x.Gender).NotEmpty().MaximumLength(15)
        .Must(x => x == GenderType.Male || x == GenderType.Female || x == GenderType.Other)
        .WithMessage($"'Gender' must be {GenderType.Male} / {GenderType.Female} / {GenderType.Other}");

      RuleFor(x => x.Photos).NotEmpty()
        .Must(x => x != null && x.Count <= 3)
        .WithMessage("'Photos' must contain fewer than 3 items.");

      RuleForEach(x => x.Photos).SetValidator(new UpdateProfilePhotosValidator());
    }
  }

  public class UpdateProfilePhotosValidator : AbstractValidator<UpdateProfilePhotos>
  {
    public UpdateProfilePhotosValidator()
    {
      RuleFor(x => x.Url).NotEmpty().MaximumLength(2048);
    }
  }
}

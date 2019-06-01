using System;
using FluentValidation;

namespace Skelvy.Application.Meetings.Commands.CreateMeetingRequest
{
  public class CreateMeetingRequestCommandValidator : AbstractValidator<CreateMeetingRequestCommand>
  {
    public CreateMeetingRequestCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();

      RuleFor(x => x.MinDate).NotEmpty()
        .Must(x => x >= DateTimeOffset.UtcNow.AddDays(-1))
        .WithMessage("'MinDate' must show the future.");
      RuleFor(x => x.MaxDate).NotEmpty()
        .Unless(x => x.MaxDate >= x.MinDate)
        .WithMessage("'MaxDate' must be after 'MinDate'.");

      RuleFor(x => x.MinAge).NotEmpty()
        .Must(x => x >= 18)
        .WithMessage("'MinAge' must show the age of majority.");
      RuleFor(x => x.MaxAge).NotEmpty()
        .Unless(x => x.MaxAge >= x.MinAge)
        .WithMessage("'MaxAge' must be bigger than 'MinAge'.");
      RuleFor(x => x.MaxAge).NotEmpty()
        .Unless(x => x.MaxAge - x.MinAge >= 5)
        .WithMessage("Age difference must be more or equal to 5 years")
        .Must(x => x <= 55)
        .WithMessage("'MaxAge' must be less or equal 55.");

      RuleFor(x => x.Latitude).NotEmpty();
      RuleFor(x => x.Longitude).NotEmpty();

      RuleFor(x => x.DrinkTypes).NotEmpty();
      RuleForEach(x => x.DrinkTypes).SetValidator(new CreateMeetingRequestPhotosValidator());
    }
  }

  public class CreateMeetingRequestPhotosValidator : AbstractValidator<CreateMeetingRequestDrinkType>
  {
    public CreateMeetingRequestPhotosValidator()
    {
      RuleFor(x => x.Id).NotEmpty();
    }
  }
}

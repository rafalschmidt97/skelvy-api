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
        .Must(x => x > DateTime.Now.Date)
        .WithMessage("'MinDate' must show the future.");
      RuleFor(x => x.MaxDate).NotEmpty()
        .Unless(x => x.MaxDate > x.MinDate)
        .WithMessage("'MaxDate' must be after 'MinDate'.");

      RuleFor(x => x.MinAge).NotEmpty()
        .Must(x => x >= 18)
        .WithMessage("'MinAge' must show the age of majority.");
      RuleFor(x => x.MaxAge).NotEmpty()
        .Unless(x => x.MaxAge > x.MinAge)
        .WithMessage("'MaxAge' must be bigger than 'MinAge'.");
      RuleFor(x => x.MaxAge).NotEmpty()
        .Unless(x => x.MaxAge - x.MinAge >= 5)
        .WithMessage("Age difference must be more or equal to 5 years");

      RuleFor(x => x.Latitude).NotEmpty();
      RuleFor(x => x.Longitude).NotEmpty();

      RuleFor(x => x.Drinks).NotEmpty();
      RuleForEach(x => x.Drinks).SetValidator(new CreateMeetingRequestPhotosValidator());
    }
  }

  public class CreateMeetingRequestPhotosValidator : AbstractValidator<CreateMeetingRequestDrink>
  {
    public CreateMeetingRequestPhotosValidator()
    {
      RuleFor(x => x.Id).NotEmpty();
    }
  }
}

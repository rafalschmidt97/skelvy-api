using FluentValidation;

namespace Skelvy.Application.Users.Commands.AddUserDevice
{
  public class AddUserDeviceCommandValidator : AbstractValidator<AddUserDeviceCommand>
  {
    public AddUserDeviceCommandValidator()
    {
      RuleFor(x => x.UserId).NotEmpty();
      RuleFor(x => x.RegistrationId).NotEmpty().MaximumLength(250);
    }
  }
}

using Destructurama.Attributed;
using MediatR;

namespace Skelvy.Application.Users.Commands.AddUserDevice
{
  public class AddUserDeviceCommand : IRequest
  {
    public int UserId { get; set; }

    [LogMasked]
    public string RegistrationId { get; set; }
  }
}

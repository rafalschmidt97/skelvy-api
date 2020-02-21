using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.UpdateUserEmail
{
  public class UpdateUserEmailCommand : ICommand
  {
    public UpdateUserEmailCommand(int userId, string email)
    {
      UserId = userId;
      Email = email;
    }

    [JsonConstructor]
    public UpdateUserEmailCommand()
    {
    }

    public int UserId { get; set; }
    public string Email { get; set; }
  }
}

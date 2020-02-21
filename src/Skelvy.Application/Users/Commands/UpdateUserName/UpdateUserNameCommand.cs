using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Users.Commands.UpdateUserName
{
  public class UpdateUserNameCommand : ICommand
  {
    public UpdateUserNameCommand(int userId, string name)
    {
      UserId = userId;
      Name = name;
    }

    [JsonConstructor]
    public UpdateUserNameCommand()
    {
    }

    public int UserId { get; set; }
    public string Name { get; set; }
  }
}

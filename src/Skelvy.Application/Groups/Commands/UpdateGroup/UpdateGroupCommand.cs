using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Groups.Commands.UpdateGroup
{
  public class UpdateGroupCommand : ICommand
  {
    public UpdateGroupCommand(int userId, int groupId, string name)
    {
      UserId = userId;
      GroupId = groupId;
      Name = name;
    }

    [JsonConstructor]
    public UpdateGroupCommand()
    {
    }

    public int UserId { get; set; }
    public int GroupId { get; set; }
    public string Name { get; set; }
  }
}

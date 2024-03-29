using Newtonsoft.Json;
using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Groups.Commands.LeaveGroup
{
  public class LeaveGroupCommand : ICommand
  {
    public LeaveGroupCommand(int groupId, int userId)
    {
      GroupId = groupId;
      UserId = userId;
    }

    [JsonConstructor]
    public LeaveGroupCommand()
    {
    }

    public int GroupId { get; set; }
    public int UserId { get; set; }
  }
}

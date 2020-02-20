using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Groups.Commands.RemoveUserFromGroup
{
  public class RemoveUserFromGroupCommand : ICommand
  {
    public RemoveUserFromGroupCommand(int userId, int groupId, int removingUserId)
    {
      UserId = userId;
      GroupId = groupId;
      RemovingUserId = removingUserId;
    }

    public int UserId { get; set; }
    public int GroupId { get; set; }
    public int RemovingUserId { get; set; }
  }
}

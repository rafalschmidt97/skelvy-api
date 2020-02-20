using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Groups.Commands.UpdateGroupUserRole
{
  public class UpdateGroupUserRoleCommand : ICommand
  {
    public UpdateGroupUserRoleCommand(int userId, int groupId, int updatedUserId, string role)
    {
      UserId = userId;
      GroupId = groupId;
      UpdatedUserId = updatedUserId;
      Role = role;
    }

    public int UserId { get; set; }
    public int GroupId { get; set; }
    public int UpdatedUserId { get; set; }
    public string Role { get; set; }
  }
}

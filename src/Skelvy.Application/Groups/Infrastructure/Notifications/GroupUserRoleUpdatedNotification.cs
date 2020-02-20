using System.Collections.Generic;

namespace Skelvy.Application.Groups.Infrastructure.Notifications
{
  public class GroupUserRoleUpdatedNotification
  {
    public GroupUserRoleUpdatedNotification(int groupId, int userId, int updatedUserId, string role, IEnumerable<int> usersId)
    {
      GroupId = groupId;
      UserId = userId;
      UpdatedUserId = updatedUserId;
      Role = role;
      UsersId = usersId;
    }

    public int GroupId { get; private set; }
    public int UserId { get; private set; }
    public int UpdatedUserId { get; private set; }
    public string Role { get; private set; }
    public IEnumerable<int> UsersId { get; private set; }
  }
}

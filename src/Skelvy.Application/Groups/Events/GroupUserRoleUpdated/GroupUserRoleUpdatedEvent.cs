using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Groups.Events.GroupUserRoleUpdated
{
  public class GroupUserRoleUpdatedEvent : IEvent
  {
    public GroupUserRoleUpdatedEvent(int userId, int groupId, int updatedUserId, string role)
    {
      UserId = userId;
      GroupId = groupId;
      UpdatedUserId = updatedUserId;
      Role = role;
    }

    public int UserId { get; private set; }
    public int GroupId { get; private set; }
    public int UpdatedUserId { get; private set; }
    public string Role { get; private set; }
  }
}

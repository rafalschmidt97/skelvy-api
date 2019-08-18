using Skelvy.Application.Core.Bus;

namespace Skelvy.Application.Meetings.Commands.UpdateMeetingUserRole
{
  public class UpdateMeetingUserRoleCommand : ICommand
  {
    public UpdateMeetingUserRoleCommand(int userId, int meetingId, int updatedUserId, string role)
    {
      UserId = userId;
      MeetingId = meetingId;
      UpdatedUserId = updatedUserId;
      Role = role;
    }

    public int UserId { get; set; }
    public int MeetingId { get; set; }
    public int UpdatedUserId { get; set; }
    public string Role { get; set; }
  }
}

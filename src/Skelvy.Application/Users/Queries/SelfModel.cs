using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Users.Queries
{
  public class SelfModel
  {
    public SelfModel(UserDto user, MeetingModel meetingModel)
    {
      User = user;
      MeetingModel = meetingModel;
    }

    public SelfModel(UserDto user)
    {
      User = user;
    }

    public UserDto User { get; }
    public MeetingModel MeetingModel { get; }
  }
}

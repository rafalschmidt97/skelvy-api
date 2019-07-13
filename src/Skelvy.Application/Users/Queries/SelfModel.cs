using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Users.Queries
{
  public class SelfModel
  {
    public SelfModel(SelfUserDto user, MeetingModel meetingModel)
    {
      User = user;
      MeetingModel = meetingModel;
    }

    public SelfModel(SelfUserDto user)
    {
      User = user;
    }

    public SelfUserDto User { get; }
    public MeetingModel MeetingModel { get; }
  }
}

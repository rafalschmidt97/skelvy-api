using Skelvy.Application.Meetings.Queries;

namespace Skelvy.Application.Users.Queries
{
  public class SelfViewModel
  {
    public SelfViewModel(UserDto user, MeetingViewModel meetingModel)
    {
      User = user;
      MeetingModel = meetingModel;
    }

    public SelfViewModel(UserDto user)
    {
      User = user;
    }

    public UserDto User { get; }
    public MeetingViewModel MeetingModel { get; }
  }
}

using Skelvy.Domain.Entities;
using Xunit;

namespace Skelvy.Domain.Test.Entities
{
  public class MeetingUserTest
  {
    [Fact]
    public void ShouldBeLeft()
    {
      var entity = new MeetingUser(1, 1, 1);
      entity.Leave();

      Assert.True(entity.IsRemoved);
      Assert.NotNull(entity.RemovedAt);
    }
  }
}

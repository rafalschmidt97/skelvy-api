using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.UpdateUserProfile;
using Skelvy.Common.Exceptions;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class UpdateUserProfileCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = Request();
      var handler = new UpdateUserProfileCommandHandler(InitializedDbContext());

      await handler.Handle(request, CancellationToken.None);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = Request();
      var handler = new UpdateUserProfileCommandHandler(DbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request, CancellationToken.None));
    }

    private static UpdateUserProfileCommand Request()
    {
      return new UpdateUserProfileCommand
      {
        UserId = 1,
        Name = "Example",
        Gender = "Female",
        Birthday = DateTimeOffset.UtcNow.AddYears(-18),
        Photos = new List<UpdateUserProfilePhotos>
        {
          new UpdateUserProfilePhotos { Url = "Url" }
        }
      };
    }
  }
}

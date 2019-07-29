using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.UpdateUserProfile;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums.Users;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class UpdateUserProfileCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = Request();
      var dbContext = InitializedDbContext();
      var handler = new UpdateUserProfileCommandHandler(
        new UserProfilesRepository(dbContext),
        new UserProfilePhotosRepository(dbContext),
        new AttachmentsRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = Request();
      var dbContext = DbContext();
      var handler = new UpdateUserProfileCommandHandler(
        new UserProfilesRepository(dbContext),
        new UserProfilePhotosRepository(dbContext),
        new AttachmentsRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    private static UpdateUserProfileCommand Request()
    {
      return new UpdateUserProfileCommand(
        1,
        "Example",
        DateTimeOffset.UtcNow.AddYears(-18),
        GenderTypes.Female,
        null,
        new List<UpdateUserProfilePhotos>
        {
          new UpdateUserProfilePhotos("Url"),
        });
    }
  }
}

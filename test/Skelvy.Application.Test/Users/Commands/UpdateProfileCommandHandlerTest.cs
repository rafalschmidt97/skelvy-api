using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.UpdateProfile;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class UpdateProfileCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = Request();
      var dbContext = InitializedDbContext();
      var handler = new UpdateProfileCommandHandler(
        new ProfilesRepository(dbContext),
        new ProfilePhotosRepository(dbContext),
        new AttachmentsRepository(dbContext));

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = Request();
      var dbContext = DbContext();
      var handler = new UpdateProfileCommandHandler(
        new ProfilesRepository(dbContext),
        new ProfilePhotosRepository(dbContext),
        new AttachmentsRepository(dbContext));

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }

    private static UpdateProfileCommand Request()
    {
      return new UpdateProfileCommand(
        1,
        "Example",
        DateTimeOffset.UtcNow.AddYears(-18),
        GenderType.Female,
        null,
        new List<UpdateProfilePhotos>
        {
          new UpdateProfilePhotos("Url"),
        });
    }
  }
}

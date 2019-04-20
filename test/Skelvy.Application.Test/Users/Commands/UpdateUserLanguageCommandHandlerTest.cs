using System.Threading.Tasks;
using Skelvy.Application.Users.Commands.UpdateUserLanguage;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Enums.Users;
using Xunit;

namespace Skelvy.Application.Test.Users.Commands
{
  public class UpdateUserLanguageCommandHandlerTest : RequestTestBase
  {
    [Fact]
    public async Task ShouldNotThrowException()
    {
      var request = new UpdateUserLanguageCommand(1, LanguageTypes.EN);
      var handler = new UpdateUserLanguageCommandHandler(InitializedDbContext());

      await handler.Handle(request);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new UpdateUserLanguageCommand(1, LanguageTypes.EN);
      var handler = new UpdateUserLanguageCommandHandler(DbContext());

      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

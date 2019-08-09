using System.Threading.Tasks;
using Moq;
using Skelvy.Application.Meetings.Queries;
using Skelvy.Application.Users.Queries;
using Skelvy.Application.Users.Queries.FindSelf;
using Skelvy.Common.Exceptions;
using Skelvy.Domain.Entities;
using Skelvy.Domain.Enums.Users;
using Skelvy.Persistence.Repositories;
using Xunit;

namespace Skelvy.Application.Test.Users.Queries
{
  public class FindSelfQueryHandlerTest : RequestTestBase
  {
    private readonly Mock<IMeetingMapper> _mapper;

    public FindSelfQueryHandlerTest()
    {
      _mapper = new Mock<IMeetingMapper>();
    }

    [Fact]
    public async Task ShouldReturnModel()
    {
      var request = new FindSelfQuery(1, LanguageType.EN);
      var dbContext = InitializedDbContext();
      _mapper.Setup(x =>
          x.Map(It.IsAny<User>(), It.IsAny<MeetingRequest>(), It.IsAny<string>()))
        .ReturnsAsync(new SelfModel(null, null));

      var handler = new FindSelfQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MessagesRepository(dbContext),
        _mapper.Object);

      var result = await handler.Handle(request);

      Assert.IsType<SelfModel>(result);
    }

    [Fact]
    public async Task ShouldThrowException()
    {
      var request = new FindSelfQuery(1, LanguageType.EN);
      var dbContext = DbContext();
      var handler = new FindSelfQueryHandler(
        new UsersRepository(dbContext),
        new MeetingRequestsRepository(dbContext),
        new MeetingsRepository(dbContext),
        new MessagesRepository(dbContext),
        _mapper.Object);
      await Assert.ThrowsAsync<NotFoundException>(() =>
        handler.Handle(request));
    }
  }
}

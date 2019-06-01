using Skelvy.Persistence.Repositories;

namespace Skelvy.Application.Test
{
  public abstract class DatabaseRequestTestBase : RequestTestBase
  {
    protected static UsersRepository UsersRepository(bool initialized = true)
    {
      return initialized ?
        new UsersRepository(InitializedDbContext()) :
        new UsersRepository(DbContext());
    }

    protected static DrinkTypesRepository DrinkTypesRepository(bool initialized = true)
    {
      return initialized ?
        new DrinkTypesRepository(InitializedDbContext()) :
        new DrinkTypesRepository(DbContext());
    }

    protected static MeetingChatMessagesRepository MeetingChatMessagesRepository(bool initialized = true)
    {
      return initialized ?
        new MeetingChatMessagesRepository(InitializedDbContext()) :
        new MeetingChatMessagesRepository(DbContext());
    }

    protected static MeetingRequestsRepository MeetingRequestsRepository(bool initialized = true)
    {
      return initialized ?
        new MeetingRequestsRepository(InitializedDbContext()) :
        new MeetingRequestsRepository(DbContext());
    }

    protected static MeetingsRepository MeetingsRepository(bool initialized = true)
    {
      return initialized ?
        new MeetingsRepository(InitializedDbContext()) :
        new MeetingsRepository(DbContext());
    }

    protected static MeetingUsersRepository MeetingUsersRepository(bool initialized = true)
    {
      return initialized ?
        new MeetingUsersRepository(InitializedDbContext()) :
        new MeetingUsersRepository(DbContext());
    }
  }
}

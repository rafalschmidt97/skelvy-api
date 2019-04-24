using Skelvy.Application.Core.Persistence;

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

    protected static AuthRepository AuthRepository(bool initialized = true)
    {
      return initialized ?
        new AuthRepository(InitializedDbContext()) :
        new AuthRepository(DbContext());
    }

    protected static DrinksRepository DrinksRepository(bool initialized = true)
    {
      return initialized ?
        new DrinksRepository(InitializedDbContext()) :
        new DrinksRepository(DbContext());
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

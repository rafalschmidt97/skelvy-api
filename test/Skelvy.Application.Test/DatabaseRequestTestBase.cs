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

    protected static MessagesRepository MessagesRepository(bool initialized = true)
    {
      return initialized ?
        new MessagesRepository(InitializedDbContext()) :
        new MessagesRepository(DbContext());
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

    protected static GroupUsersRepository MeetingUsersRepository(bool initialized = true)
    {
      return initialized ?
        new GroupUsersRepository(InitializedDbContext()) :
        new GroupUsersRepository(DbContext());
    }
  }
}

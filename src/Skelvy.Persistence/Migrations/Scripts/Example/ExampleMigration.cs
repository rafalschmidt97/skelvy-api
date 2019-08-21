using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Skelvy.Persistence.Migrations.Scripts.DrinkTypesInsteadOfDrinks
{
  public class ExampleMigration : IMigration
  {
    private const string ConnectionStringName = "SKELVY_SQL_CONNECTION";
    private readonly string _connectionString;

    public ExampleMigration(IConfiguration configuration)
    {
      _connectionString = configuration[ConnectionStringName];
    }

    public async Task Migrate()
    {
      using (var connection = new SqlConnection(_connectionString))
      {
        await connection.OpenAsync();

        using (var transaction = connection.BeginTransaction())
        {
          await MeetingRequestDrinksToTypes(connection, transaction);
          await MeetingsDrinkToTypes(connection, transaction);

          transaction.Commit();
        }
      }
    }

    private static async Task MeetingRequestDrinksToTypes(
      IDbConnection connection,
      IDbTransaction transaction)
    {
      var oldMeetingRequestDrinks = await connection.QueryAsync(
        @"SELECT * FROM [MeetingRequestDrinks]", transaction: transaction);

      var newMeetingRequestDrinkTypes = new List<dynamic>();

      foreach (var entity in oldMeetingRequestDrinks.ToList())
      {
        var requestDrinkId = CovertDrinkToDrinkType(entity);
        var newListContainsRequestDrinkType = newMeetingRequestDrinkTypes.Any(x =>
          x.MeetingRequestId == entity.MeetingRequestId && x.DrinkTypeId == requestDrinkId);

        if (!newListContainsRequestDrinkType)
        {
          newMeetingRequestDrinkTypes.Add(new { entity.MeetingRequestId, DrinkTypeId = requestDrinkId });
        }
      }

      foreach (var entity in newMeetingRequestDrinkTypes)
      {
        await connection.ExecuteAsync(
          @"INSERT INTO [MeetingRequestDrinkTypes] (MeetingRequestId, DrinkTypeId)
          VALUES (@MeetingRequestId, @DrinkTypeId)",
          new { entity.MeetingRequestId, entity.DrinkTypeId },
          transaction);
      }
    }

    private static async Task MeetingsDrinkToTypes(
      IDbConnection connection,
      IDbTransaction transaction)
    {
      var meetings = await connection.QueryAsync(
        @"SELECT [Id], [DrinkId] FROM [Meetings]", transaction: transaction);

      foreach (var entity in meetings)
      {
        var meetingDrinkId = CovertDrinkToDrinkType(entity);

        await connection.ExecuteAsync(
          @"Update [Meetings] SET DrinkTypeId = @DrinkTypeId WHERE [Id] = @Id",
          new { entity.Id, DrinkTypeId = meetingDrinkId },
          transaction);
      }
    }

    private static dynamic CovertDrinkToDrinkType(dynamic entity)
    {
      return entity.DrinkId == 1 || entity.DrinkId == 2 || entity.DrinkId == 3 ? 1 : 2;
    }
  }
}

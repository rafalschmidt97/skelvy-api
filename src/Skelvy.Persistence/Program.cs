using System.IO;
using Microsoft.Extensions.Configuration;
using Skelvy.Persistence.Migrations.Scripts.DrinkTypesInsteadOfDrinks;

namespace Skelvy.Persistence
{
  public static class Program
  {
    public static void Main()
    {
      var migration = new DrinkTypesInsteadOfDrinksMigration(GetConfiguration());
      migration.Migrate().Wait();
    }

    private static IConfiguration GetConfiguration()
    {
      var basePath = string.Format(
        "{0}{1}..{1}Skelvy.WebAPI",
        Directory.GetCurrentDirectory(),
        Path.DirectorySeparatorChar);

      return new ConfigurationBuilder()
        .SetBasePath(basePath)
        .AddJsonFile("appsettings.Development.json")
        .AddEnvironmentVariables()
        .Build();
    }
  }
}

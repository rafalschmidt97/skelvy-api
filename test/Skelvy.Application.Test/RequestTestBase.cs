using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Mappers;
using Skelvy.Persistence;

namespace Skelvy.Application.Test
{
  public abstract class RequestTestBase
  {
    protected static SkelvyContext DbContext(TestDbContextTypes type = TestDbContextTypes.SqLite)
    {
      var builder = new DbContextOptionsBuilder<SkelvyContext>();

      if (type == TestDbContextTypes.SqLite)
      {
        builder.UseSqlite("DataSource=:memory:");
      }
      else if (type == TestDbContextTypes.SqlServer)
      {
        builder.UseSqlServer(
          "Server=localhost, 1433; Database=skelvy_integration; MultipleActiveResultSets=true; User Id=sa; Password=zaq1@WSX;");
      }
      else
      {
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
      }

      var dbContext = new SkelvyContext(builder.Options);

      if (type == TestDbContextTypes.SqLite)
      {
        dbContext.Database.OpenConnection();
      }
      else if (type == TestDbContextTypes.SqlServer)
      {
        dbContext.Database.Migrate();
      }

      dbContext.Database.EnsureCreated();

      return dbContext;
    }

    protected static SkelvyContext InitializedDbContext(TestDbContextTypes type = TestDbContextTypes.SqLite)
    {
      var context = DbContext(type);
      SkelvyInitializer.Initialize(context);
      return context;
    }

    protected static IMapper Mapper()
    {
      var mapperConfiguration = new MapperConfiguration(configuration =>
      {
        configuration.AddProfile(new MappingProfile());
      });

      return mapperConfiguration.CreateMapper();
    }
  }
}

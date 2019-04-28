using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Skelvy.Application.Core.Mappers;
using Skelvy.Persistence;

namespace Skelvy.Application.Test
{
  public abstract class RequestTestBase
  {
    protected static SkelvyContext DbContext(bool sqlLite = true)
    {
      var builder = new DbContextOptionsBuilder<SkelvyContext>();

      if (sqlLite)
      {
        builder.UseSqlite("DataSource=:memory:");
      }
      else
      {
        builder.UseInMemoryDatabase(Guid.NewGuid().ToString());
      }

      var dbContext = new SkelvyContext(builder.Options);

      if (sqlLite)
      {
        dbContext.Database.OpenConnection();
      }

      dbContext.Database.EnsureCreated();

      return dbContext;
    }

    protected static SkelvyContext InitializedDbContext(bool sqlLite = true)
    {
      var context = DbContext(sqlLite);
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

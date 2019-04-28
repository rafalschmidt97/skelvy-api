using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Skelvy.Persistence.Extensions;

namespace Skelvy.Persistence
{
  public static class SkelvyContextExtensions
  {
    public static async Task<List<T>> SqlQuery<T>(
      this SkelvyContext context,
      string query,
      object parameters,
      Func<DbDataReader, T> mapper)
    {
      using (var command = context.Database.GetDbConnection().CreateCommand())
      {
        command.CommandText = query;
        command.CommandType = CommandType.Text;
        context.Database.OpenConnection();

        if (parameters != null)
        {
          var parametersDictionary = parameters.GetType()
            .GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .ToDictionary(prop => prop.Name, prop => prop.GetValue(parameters, null));

          foreach (var x in parametersDictionary)
          {
            command.Parameters.Add(new SqlParameter(x.Key, x.Value));
          }
        }

        using (var result = await command.ExecuteReaderAsync())
        {
          var entities = new List<T>();

          if (result.HasRows)
          {
            if (mapper == null)
            {
              while (await result.ReadAsync())
              {
                entities.Add(result.Convert<T>());
              }
            }
            else
            {
              while (await result.ReadAsync())
              {
                entities.Add(mapper(result));
              }
            }
          }

          return entities;
        }
      }
    }

    public static async Task<List<T>> SqlQuery<T>(
      this SkelvyContext context,
      string query,
      object parameters)
    {
      return await SqlQuery<T>(context, query, parameters, null);
    }

    public static async Task<List<T>> SqlQuery<T>(
      this SkelvyContext context,
      string query,
      Func<DbDataReader, T> mapper)
    {
      return await SqlQuery(context, query, null, mapper);
    }

    public static async Task<List<T>> SqlQuery<T>(
      this SkelvyContext context,
      string query)
    {
      return await SqlQuery<T>(context, query, null, null);
    }

    public static async Task<int> SqlCommand(this SkelvyContext context, string command)
    {
      return await context.Database.ExecuteSqlCommandAsync(command);
    }
  }
}

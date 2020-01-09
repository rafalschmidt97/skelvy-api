using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace Skelvy.Persistence.Migrations.Scripts.CopyUsers
{
  public class CopyUsersMigration : IMigration
  {
    private const string _fromConnectionString = "SKELVY_SQL_CONNECTION";
    private const string _toConnectionString = "SKELVY_SQL_CONNECTION";

    public async Task Migrate()
    {
      var fromConnection = new SqlConnection(_fromConnectionString);
      var toConnection = new SqlConnection(_toConnectionString);

      await fromConnection.OpenAsync();
      await toConnection.OpenAsync();

      await CopyUsers(fromConnection, toConnection);
      await CopyProfiles(fromConnection, toConnection);
      await CopyProfilePhotos(fromConnection, toConnection);
    }

    private static async Task CopyUsers(SqlConnection fromConnection, SqlConnection toConnection)
    {
      await toConnection.ExecuteAsync(@"SET IDENTITY_INSERT Users ON");
      using (var transaction = toConnection.BeginTransaction())
      {
        var fromUsers = await fromConnection.QueryAsync<MigrationUser>(@"SELECT * FROM [Users]");

        int nameCounter = 0;

        foreach (var entity in fromUsers.ToList())
        {
          await toConnection.ExecuteAsync(
            @"INSERT INTO [Users] (Id, Email, Name, Language, FacebookId, GoogleId, CreatedAt, ModifiedAt, IsRemoved, ForgottenAt,
                     IsDisabled, DisabledReason)
            VALUES (@Id, @Email, @Name, @Language, @FacebookId, @GoogleId, @CreatedAt, @ModifiedAt, @IsRemoved, @ForgottenAt,
                    @IsDisabled, @DisabledReason)",
            new MigrationUser
            {
              Id = entity.Id,
              Email = entity.Email,
              Name = nameCounter.ToString(),
              Language = entity.Language,
              FacebookId = entity.FacebookId,
              GoogleId = entity.GoogleId,
              CreatedAt = entity.CreatedAt,
              ModifiedAt = entity.ModifiedAt,
              IsRemoved = entity.IsRemoved,
              ForgottenAt = entity.ForgottenAt,
              IsDisabled = entity.IsDisabled,
              DisabledReason = entity.DisabledReason,
            },
            transaction);

          nameCounter++;
        }

        transaction.Commit();
      }

      await toConnection.ExecuteAsync(@"SET IDENTITY_INSERT Users OFF");
    }

    private static async Task CopyProfiles(SqlConnection fromConnection, SqlConnection toConnection)
    {
      await toConnection.ExecuteAsync(@"SET IDENTITY_INSERT Profiles ON");
      using (var transaction = toConnection.BeginTransaction())
      {
        var fromProfiles = await fromConnection.QueryAsync<MigrationProfile>(@"SELECT * FROM [UserProfiles]");

        foreach (var entity in fromProfiles.ToList())
        {
          await toConnection.ExecuteAsync(
            @"INSERT INTO [Profiles] (Id, Name, Birthday, Gender, Description, ModifiedAt, UserId)
                VALUES (@Id, @Name, @Birthday, @Gender, @Description, @ModifiedAt, @UserId)",
            new MigrationProfile
            {
              Id = entity.Id,
              Name = entity.Name,
              Birthday = entity.Birthday,
              Gender = entity.Gender,
              Description = entity.Description,
              ModifiedAt = entity.ModifiedAt,
              UserId = entity.UserId,
            }, transaction);
        }

        transaction.Commit();
      }

      await toConnection.ExecuteAsync(@"SET IDENTITY_INSERT Profiles OFF");
    }

    private static async Task CopyProfilePhotos(SqlConnection fromConnection, SqlConnection toConnection)
    {
      using (var transaction = toConnection.BeginTransaction())
      {
        var fromPhotos = await fromConnection.QueryAsync<OldMigrationProfilePhoto>(@"SELECT * FROM [UserProfilePhotos]");

        foreach (var entity in fromPhotos.ToList())
        {
          var attachmentIds = await toConnection.QueryAsync<int>(
            @"INSERT INTO [Attachments] (Type, Url, CreatedAt)
            VALUES (@Type, @Url, @CreatedAt); 
            SELECT CAST(SCOPE_IDENTITY() as int)",
            new MigrationAttachment
            {
              Type = "image",
              Url = entity.Url,
              CreatedAt = DateTimeOffset.UtcNow,
            },
            transaction);

          var attachmentId = attachmentIds.Single();

          await toConnection.ExecuteAsync(
            @"INSERT INTO [ProfilePhotos] (AttachmentId, [Order], ProfileId)
            VALUES (@AttachmentId, @Order, @ProfileId)",
            new MigrationProfilePhoto
            {
              AttachmentId = attachmentId,
              Order = entity.Order,
              ProfileId = entity.ProfileId,
            },
            transaction);
        }

        transaction.Commit();
      }
    }
  }

  public class MigrationUser
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Language { get; set; }
    public string FacebookId { get; set; }
    public string GoogleId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
    public DateTimeOffset? ForgottenAt { get; set; }
    public bool IsDisabled { get; set; }
    public string DisabledReason { get; set; }
  }

  public class MigrationProfile
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Birthday { get; set; }
    public string Gender { get; set; }
    public string Description { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public int UserId { get; set; }
  }

  public class OldMigrationProfilePhoto
  {
    public int Id { get; set; }
    public string Url { get; set; }
    public int Order { get; set; }
    public int ProfileId { get; set; }
  }

  public class MigrationProfilePhoto
  {
    public int Id { get; set; }
    public int AttachmentId { get; set; }
    public int Order { get; set; }
    public int ProfileId { get; set; }
  }

  public class MigrationAttachment
  {
    public int Id { get; set; }
    public string Type { get; set; }
    public string Url { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}

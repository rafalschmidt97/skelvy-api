IF NOT EXISTS(SELECT *
              FROM [__EFMigrationsHistory]
              WHERE [MigrationId] = N'20190531102127_DrinkTypesInsteadOfDrinks')
  BEGIN
    DROP INDEX [IX_Meetings_DrinkId] ON [Meetings];
    ALTER TABLE [Meetings] DROP CONSTRAINT [FK_Meetings_Drinks_DrinkId];
    ALTER TABLE [Meetings] DROP COLUMN [DrinkId];
    ALTER TABLE [Meetings] ALTER COLUMN [DrinkTypeId] int NOT NULL;
    CREATE INDEX [IX_Meetings_DrinkTypeId] ON [Meetings] ([DrinkTypeId]);

    DROP TABLE [MeetingRequestDrinks];
    DROP TABLE [Drinks];

    DROP TABLE [Meetings_BACKUP];
    DROP TABLE [MeetingsRequestDrinks_BACKUP];
    DROP TABLE [Drinks_Backup];

    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20190531102127_DrinkTypesInsteadOfDrinks', N'2.2.4-servicing-10062');
  END;
GO

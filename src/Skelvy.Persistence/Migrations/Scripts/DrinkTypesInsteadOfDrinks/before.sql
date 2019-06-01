IF NOT EXISTS(SELECT *
              FROM [__EFMigrationsHistory]
              WHERE [MigrationId] = N'20190531102127_DrinkTypesInsteadOfDrinks')
  BEGIN
    SELECT * INTO [Meetings_BACKUP] FROM [Meetings];
    SELECT * INTO [MeetingsRequestDrinks_BACKUP] FROM [Meetings_BACKUP];
    SELECT * INTO [Drinks_Backup] FROM [Drinks];

    CREATE TABLE [DrinkTypes]
    (
      [Id]   int          NOT NULL IDENTITY,
      [Name] nvarchar(50) NOT NULL,
      CONSTRAINT [PK_DrinkTypes] PRIMARY KEY ([Id])
    );
    CREATE UNIQUE INDEX [IX_DrinkTypes_Name] ON [DrinkTypes] ([Name]);

    ALTER TABLE [Meetings] ADD [DrinkTypeId] int NULL;
    ALTER TABLE [Meetings] ADD CONSTRAINT [FK_Meetings_DrinkTypes_DrinkTypeId] FOREIGN KEY ([DrinkTypeId]) REFERENCES [DrinkTypes] ([Id]) ON DELETE NO ACTION;

    CREATE TABLE [MeetingRequestDrinkTypes]
    (
      [MeetingRequestId] int NOT NULL,
      [DrinkTypeId]      int NOT NULL,
      CONSTRAINT [PK_MeetingRequestDrinkTypes] PRIMARY KEY ([MeetingRequestId], [DrinkTypeId]),
      CONSTRAINT [FK_MeetingRequestDrinkTypes_DrinkTypes_DrinkTypeId] FOREIGN KEY ([DrinkTypeId]) REFERENCES [DrinkTypes] ([Id]) ON DELETE NO ACTION,
      CONSTRAINT [FK_MeetingRequestDrinkTypes_MeetingRequests_MeetingRequestId] FOREIGN KEY ([MeetingRequestId]) REFERENCES [MeetingRequests] ([Id]) ON DELETE NO ACTION
    );
    CREATE INDEX [IX_MeetingRequestDrinkTypes_DrinkTypeId] ON [MeetingRequestDrinkTypes] ([DrinkTypeId]);

    INSERT INTO [DrinkTypes] (Name) VALUES ('soft drinks'), ('alcoholic drinks');
  END;
GO

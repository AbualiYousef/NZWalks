IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016185728_Initial Migration')
BEGIN
    CREATE TABLE [Difficulties] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Difficulties] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016185728_Initial Migration')
BEGIN
    CREATE TABLE [Regions] (
        [Id] uniqueidentifier NOT NULL,
        [Code] nvarchar(max) NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [RegionImageUrl] nvarchar(max) NULL,
        CONSTRAINT [PK_Regions] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016185728_Initial Migration')
BEGIN
    CREATE TABLE [Walks] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [LengthInKm] float NOT NULL,
        [WalkImageUrl] nvarchar(max) NULL,
        [DifficultyID] uniqueidentifier NOT NULL,
        [RegionId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Walks] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Walks_Difficulties_DifficultyID] FOREIGN KEY ([DifficultyID]) REFERENCES [Difficulties] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Walks_Regions_RegionId] FOREIGN KEY ([RegionId]) REFERENCES [Regions] ([Id]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016185728_Initial Migration')
BEGIN
    CREATE INDEX [IX_Walks_DifficultyID] ON [Walks] ([DifficultyID]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016185728_Initial Migration')
BEGIN
    CREATE INDEX [IX_Walks_RegionId] ON [Walks] ([RegionId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231016185728_Initial Migration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231016185728_Initial Migration', N'7.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231022010156_Seeding Data For Difficulties and Regions')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Difficulties]'))
        SET IDENTITY_INSERT [Difficulties] ON;
    EXEC(N'INSERT INTO [Difficulties] ([Id], [Name])
    VALUES (''54466f17-02af-48e7-8ed3-5a4a8bfacf6f'', N''Easy''),
    (''ea294873-7a8c-4c0f-bfa7-a2eb492cbf8c'', N''Medium''),
    (''f808ddcd-b5e5-4d80-b732-1ca523e48434'', N''Hard'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Difficulties]'))
        SET IDENTITY_INSERT [Difficulties] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231022010156_Seeding Data For Difficulties and Regions')
BEGIN
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Name', N'RegionImageUrl') AND [object_id] = OBJECT_ID(N'[Regions]'))
        SET IDENTITY_INSERT [Regions] ON;
    EXEC(N'INSERT INTO [Regions] ([Id], [Code], [Name], [RegionImageUrl])
    VALUES (''14ceba71-4b51-4777-9b17-46602cf66153'', N''BOP'', N''Bay Of Plenty'', NULL),
    (''6884f7d7-ad1f-4101-8df3-7a6fa7387d81'', N''NTL'', N''Northland'', NULL),
    (''906cb139-415a-4bbb-a174-1a1faf9fb1f6'', N''NSN'', N''Nelson'', N''https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1''),
    (''cfa06ed2-bf65-4b65-93ed-c9d286ddb0de'', N''WGN'', N''Wellington'', N''https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1''),
    (''f077a22e-4248-4bf6-b564-c7cf4e250263'', N''STL'', N''Southland'', NULL),
    (''f7248fc3-2585-4efb-8d1d-1c555f4087f6'', N''AKL'', N''Auckland'', N''https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1'')');
    IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Code', N'Name', N'RegionImageUrl') AND [object_id] = OBJECT_ID(N'[Regions]'))
        SET IDENTITY_INSERT [Regions] OFF;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231022010156_Seeding Data For Difficulties and Regions')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231022010156_Seeding Data For Difficulties and Regions', N'7.0.13');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231028035704_Adding Images Table')
BEGIN
    ALTER TABLE [Walks] DROP CONSTRAINT [FK_Walks_Difficulties_DifficultyID];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231028035704_Adding Images Table')
BEGIN
    EXEC sp_rename N'[Walks].[DifficultyID]', N'DifficultyId', N'COLUMN';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231028035704_Adding Images Table')
BEGIN
    EXEC sp_rename N'[Walks].[IX_Walks_DifficultyID]', N'IX_Walks_DifficultyId', N'INDEX';
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231028035704_Adding Images Table')
BEGIN
    CREATE TABLE [Images] (
        [Id] uniqueidentifier NOT NULL,
        [FileName] nvarchar(max) NOT NULL,
        [FileDescription] nvarchar(max) NULL,
        [FileExtension] nvarchar(max) NOT NULL,
        [FileSizeInBytes] bigint NOT NULL,
        [FilePath] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Images] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231028035704_Adding Images Table')
BEGIN
    ALTER TABLE [Walks] ADD CONSTRAINT [FK_Walks_Difficulties_DifficultyId] FOREIGN KEY ([DifficultyId]) REFERENCES [Difficulties] ([Id]) ON DELETE CASCADE;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20231028035704_Adding Images Table')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231028035704_Adding Images Table', N'7.0.13');
END;
GO

COMMIT;
GO


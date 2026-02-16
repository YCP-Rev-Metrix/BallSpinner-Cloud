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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251021160455_AddTables')
BEGIN
    IF SCHEMA_ID(N'combinedDB') IS NULL EXEC(N'CREATE SCHEMA [combinedDB];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251021160455_AddTables')
BEGIN
    CREATE TABLE [combinedDB].[Balls] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [Weight] nvarchar(50) NOT NULL,
        [CoreType] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_Balls] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251021160455_AddTables')
BEGIN
    CREATE TABLE [combinedDB].[Events] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        [Average] int NOT NULL,
        [Stats] int NOT NULL,
        [Standings] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Events] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251021160455_AddTables')
BEGIN
    CREATE TABLE [combinedDB].[Frames] (
        [Id] int NOT NULL IDENTITY,
        [GameId] int NOT NULL,
        [ShotOne] int NOT NULL,
        [ShotTwo] int NOT NULL,
        [FrameNumber] int NOT NULL,
        [Lane] int NOT NULL,
        [Result] int NOT NULL,
        CONSTRAINT [PK_Frames] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251021160455_AddTables')
BEGIN
    CREATE TABLE [combinedDB].[Users] (
        [Id] int NOT NULL IDENTITY,
        [Firstname] nvarchar(50) NOT NULL,
        [Lastname] nvarchar(50) NOT NULL,
        [Username] nvarchar(50) NOT NULL,
        [HashedPassword] varbinary(max) NOT NULL,
        [Email] nvarchar(max) NOT NULL,
        [PhoneNumber] nvarchar(12) NOT NULL,
        [LastLogin] nvarchar(max) NOT NULL,
        [Hand] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251021160455_AddTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251021160455_AddTables', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251029015335_UsersEstabGamesSeshShots')
BEGIN
    CREATE TABLE [combinedDB].[Establishments] (
        [ID] int NOT NULL IDENTITY,
        [Name] nvarchar(max) NOT NULL,
        [Lanes] nvarchar(max) NOT NULL,
        [Type] nvarchar(max) NOT NULL,
        [Location] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Establishments] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251029015335_UsersEstabGamesSeshShots')
BEGIN
    CREATE TABLE [combinedDB].[Games] (
        [ID] int NOT NULL IDENTITY,
        [GameNumber] nvarchar(max) NOT NULL,
        [Lanes] nvarchar(max) NOT NULL,
        [Score] int NOT NULL,
        [Win] int NOT NULL,
        [StartingLane] int NOT NULL,
        [SessionID] int NOT NULL,
        [TeamResult] int NOT NULL,
        [IndividualResult] int NOT NULL,
        CONSTRAINT [PK_Games] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251029015335_UsersEstabGamesSeshShots')
BEGIN
    CREATE TABLE [combinedDB].[Sessions] (
        [ID] int NOT NULL IDENTITY,
        [SessionNumber] int NOT NULL,
        [EstablishmentID] int NOT NULL,
        [EventID] int NOT NULL,
        [DateTime] int NOT NULL,
        [TeamOpponent] nvarchar(max) NOT NULL,
        [IndividualOpponent] nvarchar(max) NOT NULL,
        [Score] int NOT NULL,
        [Stats] int NOT NULL,
        [TeamRecord] int NOT NULL,
        [IndividualRecord] int NOT NULL,
        CONSTRAINT [PK_Sessions] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251029015335_UsersEstabGamesSeshShots')
BEGIN
    CREATE TABLE [combinedDB].[Shots] (
        [ID] int NOT NULL IDENTITY,
        [Type] int NOT NULL,
        [SmartDotID] int NOT NULL,
        [SessionID] int NOT NULL,
        [BallID] int NOT NULL,
        [FrameID] int NOT NULL,
        [ShotNumber] int NOT NULL,
        [LeaveType] int NOT NULL,
        [Side] nvarchar(max) NOT NULL,
        [Position] nvarchar(max) NOT NULL,
        [Comment] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Shots] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251029015335_UsersEstabGamesSeshShots')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251029015335_UsersEstabGamesSeshShots', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    IF SCHEMA_ID(N'Team_PI_Tables') IS NULL EXEC(N'CREATE SCHEMA [Team_PI_Tables];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[DiagnosticScript] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [Time] real NOT NULL,
        [MotorId] int NOT NULL,
        [Instruction] real NOT NULL,
        CONSTRAINT [PK_DiagnosticScript] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[EncoderData] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [Time] real NOT NULL,
        [Pulses] real NOT NULL,
        [MotorId] int NOT NULL,
        CONSTRAINT [PK_EncoderData] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[HeatData] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [Time] real NOT NULL,
        [Value] real NOT NULL,
        [MotorId] real NOT NULL,
        CONSTRAINT [PK_HeatData] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[PiSession] (
        [Id] int NOT NULL IDENTITY,
        [TimeStamp] datetime2 NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [IsShotMode] bit NOT NULL,
        CONSTRAINT [PK_PiSession] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[ShotScript] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [Time] real NOT NULL,
        [Rpm] real NOT NULL,
        [AngleDegrees] real NOT NULL,
        [TiltDegrees] real NOT NULL,
        CONSTRAINT [PK_ShotScript] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[SmartDotData] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [Time] real NOT NULL,
        [AccelX] real NOT NULL,
        [AccelY] real NOT NULL,
        [AccelZ] real NOT NULL,
        [GyroX] real NOT NULL,
        [GyroY] real NOT NULL,
        [GyroZ] real NOT NULL,
        [MagnoX] real NOT NULL,
        [MagnoY] real NOT NULL,
        [MagnoZ] real NOT NULL,
        [Light] real NOT NULL,
        CONSTRAINT [PK_SmartDotData] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251104170247_AddPiTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251104170247_AddPiTables', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251116200753_AddedPiTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251116200753_AddedPiTables', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251117231619_AddDataSelectorToSmartDotTable')
BEGIN
    ALTER TABLE [Team_PI_Tables].[SmartDotData] ADD [DataSelector] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251117231619_AddDataSelectorToSmartDotTable')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251117231619_AddDataSelectorToSmartDotTable', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204020317_AddedReplayIterationColumnToEncoderSmartDotHeat')
BEGIN
    DECLARE @var0 sysname;
    SELECT @var0 = [d].[name]
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Team_PI_Tables].[SmartDotData]') AND [c].[name] = N'DataSelector');
    IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Team_PI_Tables].[SmartDotData] DROP CONSTRAINT [' + @var0 + '];');
    ALTER TABLE [Team_PI_Tables].[SmartDotData] DROP COLUMN [DataSelector];
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204020317_AddedReplayIterationColumnToEncoderSmartDotHeat')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204020317_AddedReplayIterationColumnToEncoderSmartDotHeat', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204023329_DataSelector')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204023329_DataSelector', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204023631_ReplayIteration')
BEGIN
    ALTER TABLE [Team_PI_Tables].[SmartDotData] ADD [ReplayIteration] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204023631_ReplayIteration')
BEGIN
    ALTER TABLE [Team_PI_Tables].[HeatData] ADD [ReplayIteration] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204023631_ReplayIteration')
BEGIN
    ALTER TABLE [Team_PI_Tables].[EncoderData] ADD [ReplayIteration] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204023631_ReplayIteration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204023631_ReplayIteration', N'7.0.20');
END;
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204024742_AddedReplayIteration')
BEGIN
    ALTER TABLE [Team_PI_Tables].[SmartDotData] ADD [DataSelector] int NOT NULL DEFAULT 0;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20251204024742_AddedReplayIteration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20251204024742_AddedReplayIteration', N'7.0.20');
END;
GO

COMMIT;
GO


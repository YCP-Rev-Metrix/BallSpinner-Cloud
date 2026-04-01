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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    IF SCHEMA_ID(N'combinedDB') IS NULL EXEC(N'CREATE SCHEMA [combinedDB];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    IF SCHEMA_ID(N'Team_PI_Tables') IS NULL EXEC(N'CREATE SCHEMA [Team_PI_Tables];');
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Balls] (
        [Id] int NOT NULL IDENTITY,
        [MobileID] int NULL,
        [UserId] int NOT NULL,
        [Name] nvarchar(50) NOT NULL,
        [BallMFG] nvarchar(100) NOT NULL,
        [BallMFGName] nvarchar(100) NOT NULL,
        [SerialNumber] nvarchar(100) NOT NULL,
        [Weight] int NULL,
        [Core] nvarchar(100) NOT NULL,
        [ColorString] nvarchar(50) NOT NULL,
        [Coverstock] nvarchar(100) NOT NULL,
        [Comment] nvarchar(500) NOT NULL,
        [Enabled] bit NOT NULL,
        CONSTRAINT [PK_Balls] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[EncoderData] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [Time] real NOT NULL,
        [Pulses] real NOT NULL,
        [MotorId] int NOT NULL,
        [ReplayIteration] int NOT NULL,
        CONSTRAINT [PK_EncoderData] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Establishments] (
        [ID] int NOT NULL IDENTITY,
        [MobileID] int NULL,
        [UserId] int NULL,
        [FullName] nvarchar(100) NOT NULL,
        [NickName] nvarchar(100) NOT NULL,
        [GPSLocation] nvarchar(200) NOT NULL,
        [HomeHouse] bit NOT NULL,
        [Reason] nvarchar(200) NOT NULL,
        [Address] nvarchar(200) NOT NULL,
        [PhoneNumber] nvarchar(20) NOT NULL,
        [Lanes] nvarchar(50) NOT NULL,
        [Type] nvarchar(50) NOT NULL,
        [Location] nvarchar(100) NOT NULL,
        [Enabled] bit NOT NULL,
        CONSTRAINT [PK_Establishments] PRIMARY KEY ([ID])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Events] (
        [Id] int NOT NULL IDENTITY,
        [MobileID] int NULL,
        [UserId] int NOT NULL,
        [LongName] nvarchar(200) NOT NULL,
        [NickName] nvarchar(100) NOT NULL,
        [Type] nvarchar(50) NOT NULL,
        [Location] nvarchar(100) NOT NULL,
        [StartDate] nvarchar(20) NOT NULL,
        [EndDate] nvarchar(20) NOT NULL,
        [WeekDay] nvarchar(20) NOT NULL,
        [StartTime] nvarchar(10) NOT NULL,
        [NumGamesPerSession] int NOT NULL,
        [Average] int NULL,
        [Schedule] nvarchar(500) NOT NULL,
        [Stats] int NULL,
        [Standings] nvarchar(500) NOT NULL,
        [Enabled] bit NOT NULL,
        CONSTRAINT [PK_Events] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Frames] (
        [Id] int NOT NULL IDENTITY,
        [MobileID] int NULL,
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Games] (
        [ID] int NOT NULL IDENTITY,
        [MobileID] int NULL,
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[HeatData] (
        [Id] int NOT NULL IDENTITY,
        [SessionId] int NOT NULL,
        [Time] real NOT NULL,
        [Value] real NOT NULL,
        [MotorId] real NOT NULL,
        [ReplayIteration] int NOT NULL,
        CONSTRAINT [PK_HeatData] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [Team_PI_Tables].[PiSession] (
        [Id] int NOT NULL IDENTITY,
        [TimeStamp] datetime2 NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [IsShotMode] bit NOT NULL,
        [Spin_Instruction_Points] nvarchar(max) NOT NULL,
        [Tilt_Instruction_Points] nvarchar(max) NOT NULL,
        [Angle_Instruction_Points] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_PiSession] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Sessions] (
        [ID] int NOT NULL IDENTITY,
        [MobileID] int NULL,
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Shots] (
        [ID] int NOT NULL IDENTITY,
        [MobileID] int NULL,
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
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
        [ReplayIteration] int NOT NULL,
        [DataSelector] int NOT NULL,
        CONSTRAINT [PK_SmartDotData] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    CREATE TABLE [combinedDB].[Users] (
        [Id] int NOT NULL IDENTITY,
        [MobileID] int NULL,
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

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260401011639_NewTables')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260401011639_NewTables', N'7.0.20');
END;
GO

COMMIT;
GO

-- Migration: make optional string columns nullable on Balls, Events, and Establishments.
BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260403000000_NullableOptionalColumns')
BEGIN
    ALTER TABLE [combinedDB].[Balls] ALTER COLUMN [BallMFG]      nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Balls] ALTER COLUMN [BallMFGName]  nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Balls] ALTER COLUMN [SerialNumber] nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Balls] ALTER COLUMN [Core]         nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Balls] ALTER COLUMN [ColorString]  nvarchar(50)  NULL;
    ALTER TABLE [combinedDB].[Balls] ALTER COLUMN [Coverstock]   nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Balls] ALTER COLUMN [Comment]      nvarchar(500) NULL;

    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [LongName]           nvarchar(200) NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [NickName]           nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [Type]               nvarchar(50)  NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [Location]           nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [StartDate]          nvarchar(20)  NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [EndDate]            nvarchar(20)  NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [WeekDay]            nvarchar(20)  NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [StartTime]          nvarchar(10)  NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [Schedule]           nvarchar(500) NULL;
    ALTER TABLE [combinedDB].[Events] ALTER COLUMN [Standings]          nvarchar(500) NULL;

    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [FullName]    nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [NickName]    nvarchar(100) NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [GPSLocation] nvarchar(200) NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [Reason]      nvarchar(200) NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [Address]     nvarchar(200) NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [PhoneNumber] nvarchar(20)  NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [Lanes]       nvarchar(50)  NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [Type]        nvarchar(50)  NULL;
    ALTER TABLE [combinedDB].[Establishments] ALTER COLUMN [Location]    nvarchar(100) NULL;
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20260403000000_NullableOptionalColumns')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260403000000_NullableOptionalColumns', N'7.0.20');
END;
GO

COMMIT;
GO


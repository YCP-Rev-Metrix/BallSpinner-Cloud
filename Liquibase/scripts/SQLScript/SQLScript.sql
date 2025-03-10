--liquibase formatted sql
--changeset RevMetrix:1
USE [master]
GO
/****** Object:  Database [revmetrix-bs]    Script Date: 2/3/2025 10:08:55 PM ******/
USE [revmetrix-bs]
GO
/****** Object:  Table [dbo].[Arsenal]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Arsenal](
	[userid] [bigint] NOT NULL,
	[ball_id] [bigint] NOT NULL,
	[status] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ball]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Ball](
	[ballid] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[weight] [float] NOT NULL,
	[diameter] [float] NOT NULL,
	[core_type] [varchar](25) NULL,
 CONSTRAINT [Ball_PK] PRIMARY KEY CLUSTERED 
(
	[ballid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [BallName_UNIQUE] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocalShots]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalShots](
	[userid] [bigint] IDENTITY(1,1) NOT NULL,
	[ShotName] [varchar](50) NOT NULL,
 CONSTRAINT [UQ_User_UserId_ShotName] UNIQUE NONCLUSTERED 
(
	[userid] ASC,
	[ShotName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RefreshToken]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshToken](
	[expiration] [datetime] NOT NULL,
	[userid] [bigint] NOT NULL,
	[token] [varbinary](32) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SD_Sensor]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SD_Sensor](
	[sensor_id] [bigint] IDENTITY(1,1) NOT NULL,
	[shotid] [bigint] NOT NULL,
	[frequency] [float] NOT NULL,
	[type_id] [smallint] NOT NULL,
 CONSTRAINT [SmartDotSensor_PK] PRIMARY KEY CLUSTERED 
(
	[sensor_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SensorData]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SensorData](
	[sensor_id] [bigint] NOT NULL,
	[count] [int] NULL,
	[brightness] [float] NULL,
	[xaxis] [float] NULL,
	[yaxis] [float] NULL,
	[zaxis] [float] NULL,
	[waxis] [float] NULL,
	[logtime] [float] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SensorType]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SensorType](
	[type_id] [smallint] NOT NULL,
	[type] [varchar](20) NOT NULL,
 CONSTRAINT [typeid_PK] PRIMARY KEY CLUSTERED 
(
	[type_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SimulatedShot]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SimulatedShot](
	[shotid] [bigint] IDENTITY(1,1) NOT NULL,
	[speed] [float] NOT NULL,
	[angle] [float] NULL,
	[position] [float] NULL,
	[smartdot_sensorsid] [bigint] NULL,
	[ballspinner_sensorsid] [bigint] NULL,
	[Created] [datetime] NULL,
 CONSTRAINT [SimulatedShot_PK] PRIMARY KEY CLUSTERED 
(
	[shotid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SimulatedShotList]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SimulatedShotList](
	[userid] [bigint] NOT NULL,
	[shotid] [bigint] NOT NULL,
	[name] [varchar](30) NULL,
 CONSTRAINT [SimulatedShotList_UserName_UNIQUE] UNIQUE NONCLUSTERED 
(
	[userid] ASC,
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmartDot]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmartDot](
	[smartdot_id] [bigint] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[address] [varchar](48) NOT NULL,
 CONSTRAINT [SmartDot_PK] PRIMARY KEY CLUSTERED 
(
	[smartdot_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [SmartDot_UNIQUE] UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmartDotList]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmartDotList](
	[userid] [bigint] NOT NULL,
	[smartdot_id] [bigint] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2/3/2025 10:08:57 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[firstname] [varchar](50) NULL,
	[lastname] [varchar](50) NULL,
	[username] [varchar](50) NOT NULL,
	[password] [varbinary](64) NOT NULL,
	[salt] [varbinary](64) NOT NULL,
	[email] [varchar](50) NULL,
	[phone] [varchar](50) NULL,
	[roles] [varchar](50) NOT NULL,
 CONSTRAINT [User_PK] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [Username_UNIQUE] UNIQUE NONCLUSTERED 
(
	[username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Arsenal]  WITH CHECK ADD  CONSTRAINT [Arsenal_Ball_FK] FOREIGN KEY([ball_id])
REFERENCES [dbo].[Ball] ([ballid])
GO
ALTER TABLE [dbo].[Arsenal] CHECK CONSTRAINT [Arsenal_Ball_FK]
GO
ALTER TABLE [dbo].[Arsenal]  WITH CHECK ADD  CONSTRAINT [Arsenal_User_FK] FOREIGN KEY([userid])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[Arsenal] CHECK CONSTRAINT [Arsenal_User_FK]
GO
ALTER TABLE [dbo].[LocalShots]  WITH CHECK ADD  CONSTRAINT [LocalShot_userID_FK] FOREIGN KEY([userid])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[LocalShots] CHECK CONSTRAINT [LocalShot_userID_FK]
GO
ALTER TABLE [dbo].[RefreshToken]  WITH CHECK ADD  CONSTRAINT [FK_RefreshToken_User] FOREIGN KEY([userid])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[RefreshToken] CHECK CONSTRAINT [FK_RefreshToken_User]
GO
ALTER TABLE [dbo].[SD_Sensor]  WITH CHECK ADD  CONSTRAINT [SD_Sensor_SensorType_FK] FOREIGN KEY([type_id])
REFERENCES [dbo].[SensorType] ([type_id])
GO
ALTER TABLE [dbo].[SD_Sensor] CHECK CONSTRAINT [SD_Sensor_SensorType_FK]
GO
ALTER TABLE [dbo].[SD_Sensor]  WITH CHECK ADD  CONSTRAINT [SD_Sensor_SimulatedShot_FK] FOREIGN KEY([shotid])
REFERENCES [dbo].[SimulatedShot] ([shotid])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SD_Sensor] CHECK CONSTRAINT [SD_Sensor_SimulatedShot_FK]
GO
ALTER TABLE [dbo].[SensorData]  WITH CHECK ADD  CONSTRAINT [SampleData_SDSensor_FK] FOREIGN KEY([sensor_id])
REFERENCES [dbo].[SD_Sensor] ([sensor_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SensorData] CHECK CONSTRAINT [SampleData_SDSensor_FK]
GO
ALTER TABLE [dbo].[SimulatedShotList]  WITH CHECK ADD  CONSTRAINT [SimulatedShotList_SimulatedShot_FK] FOREIGN KEY([shotid])
REFERENCES [dbo].[SimulatedShot] ([shotid])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[SimulatedShotList] CHECK CONSTRAINT [SimulatedShotList_SimulatedShot_FK]
GO
ALTER TABLE [dbo].[SimulatedShotList]  WITH CHECK ADD  CONSTRAINT [SimulatedShotList_User_FK] FOREIGN KEY([userid])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[SimulatedShotList] CHECK CONSTRAINT [SimulatedShotList_User_FK]
GO
ALTER TABLE [dbo].[SmartDotList]  WITH CHECK ADD  CONSTRAINT [SmartDotList_SmartDot_FK] FOREIGN KEY([smartdot_id])
REFERENCES [dbo].[SmartDot] ([smartdot_id])
GO
ALTER TABLE [dbo].[SmartDotList] CHECK CONSTRAINT [SmartDotList_SmartDot_FK]
GO
ALTER TABLE [dbo].[SmartDotList]  WITH CHECK ADD  CONSTRAINT [SmartDotList_User_FK] FOREIGN KEY([userid])
REFERENCES [dbo].[User] ([id])
GO
ALTER TABLE [dbo].[SmartDotList] CHECK CONSTRAINT [SmartDotList_User_FK]
GO
/* Enter initial data */
USE [revmetrix-bs]
GO
INSERT INTO SensorType (type_id, type) VALUES 
(1, 'LightSensor'),
(2, 'Gyroscope'),
(3, 'Accelerometer'),
(4, 'Magnetometer');
-- rollback DELETE DATABASE revmetrix-bs
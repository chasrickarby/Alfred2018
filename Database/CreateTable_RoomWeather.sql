USE [Alfred]
GO

/****** Object:  Table [dbo].[RoomWeather]    Script Date: 2/10/2018 7:00:47 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[RoomWeather](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](100) NOT NULL,
	[temperature] [nvarchar](100) NOT NULL,
	[humidity] [nvarchar](100) NOT NULL,
	[time] [datetime] NOT NULL,
	[motion] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[RoomWeather] ADD  DEFAULT ((0)) FOR [temperature]
GO

ALTER TABLE [dbo].[RoomWeather] ADD  DEFAULT ((0)) FOR [humidity]
GO

ALTER TABLE [dbo].[RoomWeather] ADD  DEFAULT (getdate()) FOR [time]
GO

ALTER TABLE [dbo].[RoomWeather] ADD  DEFAULT ((0)) FOR [motion]
GO
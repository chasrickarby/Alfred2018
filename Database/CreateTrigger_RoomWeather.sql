USE [Alfred]
GO

/****** Object:  Trigger [dbo].[trgAfterUpdate]    Script Date: 2/10/2018 8:07:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TRIGGER [dbo].[trgAfterUpdate] ON [dbo].[RoomWeather]
AFTER INSERT, UPDATE 
AS
  UPDATE f set time=GETDATE() 
  FROM 
  dbo.RoomWeather AS f 
  INNER JOIN inserted 
  AS i 
  ON f.id = i.id;
GO
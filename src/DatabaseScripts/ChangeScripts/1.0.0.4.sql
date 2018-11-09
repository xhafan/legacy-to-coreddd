IF OBJECT_ID(N'Ship') IS NOT NULL
DROP TABLE Ship
GO

create table Ship
(
    ShipId int IDENTITY(1,1) NOT NULL
    , ShipName nvarchar(max)
    , Tonnage decimal(19,5)
    , ImoNumber nvarchar(max)
    , HasImoNumberBeenVerified bit
    , IsImoNumberValid bit
    CONSTRAINT PK_Ship_ShipId PRIMARY KEY CLUSTERED (ShipId ASC)
)

IF OBJECT_ID(N'ShipHistory') IS NOT NULL
DROP TABLE ShipHistory
GO

create table ShipHistory
(
    ShipHistoryId int IDENTITY(1,1) NOT NULL
    , ShipId int
    , ShipName nvarchar(max)
    , Tonnage decimal(19,5)
    , CreatedOn datetime
    CONSTRAINT PK_ShipHistory_ShipHistoryId PRIMARY KEY CLUSTERED (ShipHistoryId ASC)
)

IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID(N'UpdateShip') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE UpdateShip
GO

CREATE PROCEDURE UpdateShip
(
    @shipId int
    , @shipName nvarchar(max)
    , @tonnage decimal(19,5)
)
AS
BEGIN

update Ship set
    ShipName = @shipName
    , Tonnage = @tonnage
where ShipId = @shipId
	
INSERT INTO ShipHistory (
    ShipId
    , ShipName
    , Tonnage
    , CreatedOn
    )
VALUES (
    @shipId
    , @shipName
    , @tonnage
    , getdate()
    )

END

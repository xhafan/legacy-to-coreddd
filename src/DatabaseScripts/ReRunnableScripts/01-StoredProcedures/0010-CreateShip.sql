IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID(N'CreateShip') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE CreateShip
GO

CREATE PROCEDURE CreateShip
(
    @shipName nvarchar(max)
    , @tonnage decimal(19,5)
    , @imoNumber nvarchar(max)
)
AS
BEGIN

declare @shipId int

INSERT INTO Ship (
    ShipName
    , Tonnage
    , ImoNumber
    )
VALUES (
    @shipName
    , @tonnage
    , @imoNumber
    )
	
SELECT @shipId = SCOPE_IDENTITY()

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

select @shipId

END

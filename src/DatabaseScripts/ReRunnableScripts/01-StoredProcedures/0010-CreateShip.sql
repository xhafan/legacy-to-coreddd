IF EXISTS (SELECT * FROM SYSOBJECTS WHERE ID = OBJECT_ID(N'CreateShip') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
DROP PROCEDURE CreateShip
GO

CREATE PROCEDURE CreateShip
(
    @shipName nvarchar(max)
    , @tonnage decimal(19,5)
    , @imoNumber nvarchar(max)
    , @hasImoNumberBeenVerified bit
    , @isImoNumberValid bit
)
AS
BEGIN

declare @shipId int

INSERT INTO Ship (
    ShipName
    , Tonnage
    , ImoNumber
    , HasImoNumberBeenVerified
    , IsImoNumberValid
    )
VALUES (
    @shipName
    , @tonnage
    , @imoNumber
    , @hasImoNumberBeenVerified
    , @isImoNumberValid
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

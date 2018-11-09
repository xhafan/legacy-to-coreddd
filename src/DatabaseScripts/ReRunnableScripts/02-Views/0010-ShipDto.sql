IF OBJECT_ID(N'ShipDto') IS NOT NULL
DROP VIEW ShipDto
GO

create view ShipDto
as
select 
    ShipId      as Id
    , ShipName  as Name
    , Tonnage
    , ImoNumber
    , HasImoNumberBeenVerified
    , IsImoNumberValid
from Ship

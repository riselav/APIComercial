--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatUnidades'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatUnidades;
END;
GO
--EXEC CAT_CON_CatUnidades 0

CREATE procedure [dbo].[CAT_CON_CatUnidades] (  
@nUnidad as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nUnidad=0   
Begin  
	set nocount off 
	Select nUnidad, cDescripcion,nTipoUnidad, cClaveSAT, bActivo, cAbreviatura from Cat_Unidades  (nolock)
End   
Else  
Begin  
	set nocount off 
	Select nUnidad, cDescripcion,nTipoUnidad, cClaveSAT, bActivo, cAbreviatura from Cat_Unidades  (nolock)
	where nUnidad=@nUnidad
End  
  
End
--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatSublineas'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatSublineas;
END;
GO


CREATE procedure [dbo].[CAT_CON_CatSublineas] (  
@nSublinea as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nSublinea=0   
Begin  
	set nocount off 
	Select nSublinea, sl.cDescripcion,sl.nLinea, l.cDescripcion as cDescripcionLinea, sl.bActivo from Cat_Sublineas sl (nolock) inner join CAT_Lineas l (nolock) on sl.nLinea=l.nLinea  
End   
Else  
Begin  
	set nocount off 
	Select nSublinea, sl.cDescripcion,sl.nLinea, l.cDescripcion as cDescripcionLinea, sl.bActivo from Cat_Sublineas sl (nolock) inner join CAT_Lineas l (nolock) on sl.nLinea=l.nLinea 
	where nSublinea=@nSublinea
End  
  
End
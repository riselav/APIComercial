--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatLineas'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatLineas;
END;
GO


CREATE procedure [dbo].[CAT_CON_CatLineas] (  
@nLinea as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nLinea=0   
Begin  
	set nocount off 
	Select nLinea, cDescripcion, bActivo from CAT_Lineas (nolock)  
End   
Else  
Begin  
	set nocount off 
	Select nLinea, cDescripcion, bActivo from CAT_Lineas (nolock) where nLinea=@nLinea
End  
  
End
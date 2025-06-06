--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatMarcas'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatMarcas;
END;
GO


CREATE procedure [dbo].[CAT_CON_CatMarcas] (  
@nMarca as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nMarca=0   
Begin  
	set nocount off 
	Select nMarca, cDescripcion, bActivo from Cat_Marcas (nolock)  
End   
Else  
Begin  
	set nocount off 
	Select nMarca, cDescripcion, bActivo from Cat_Marcas (nolock) where nMarca=@nMarca
End  
  
End
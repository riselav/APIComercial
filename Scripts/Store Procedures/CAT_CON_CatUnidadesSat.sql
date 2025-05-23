--USE [Voalaft_Navola]
--GO
--EXECUTE CAT_CON_CatUnidadesSat ''
--select * from CAT_Unidades_SAT
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatUnidadesSat'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatUnidadesSat;
END;
GO


CREATE procedure [dbo].[CAT_CON_CatUnidadesSat] (  
@cClave as varchar(5)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si la clave es vacia, Consulto toda la informacion
--=================================================================================================================  
if @cClave=''   
Begin  
	set nocount off 
	Select nId,cTipo,cClave,cNombre,bActivo from CAT_Unidades_SAT (nolock) 
End   
Else  
Begin  
	set nocount off 
	Select nId,cTipo,cClave,cNombre,bActivo from CAT_Unidades_SAT (nolock) 
	where cClave=@cClave
End  
  
End
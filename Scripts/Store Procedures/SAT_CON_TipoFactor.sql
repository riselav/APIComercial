--USE [Voalaft_Navola]
--GO
--exec SAT_CON_TipoFactor ''
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'SAT_CON_TipoFactor'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE SAT_CON_TipoFactor;
END;
GO


CREATE procedure [dbo].[SAT_CON_TipoFactor] (  
@c_TipoFactor as varchar(50)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @c_TipoFactor=''   
Begin  
	set nocount off 
	Select c_TipoFactor from SAT_TipoFactor 
End   
Else  
Begin  
	set nocount off 
	Select c_TipoFactor from SAT_TipoFactor 
	where c_TipoFactor=@c_TipoFactor
End  
  
End
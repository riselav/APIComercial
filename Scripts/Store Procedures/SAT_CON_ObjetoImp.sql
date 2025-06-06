--USE [Voalaft_Navola]
--GO
--exec SAT_CON_ObjetoImp ''
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'SAT_CON_ObjetoImp'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE SAT_CON_ObjetoImp;
END;
GO


CREATE procedure [dbo].SAT_CON_ObjetoImp (  
@c_ObjetoImp as varchar(10)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @c_ObjetoImp=''   
Begin  
	set nocount off 
	Select c_ObjetoImp,Descripcion from SAT_ObjetoImp 
End   
Else  
Begin  
	set nocount off 
	Select c_ObjetoImp,Descripcion from SAT_ObjetoImp 
	where c_ObjetoImp=@c_ObjetoImp 
End  
  
End
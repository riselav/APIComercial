--USE [Voalaft_Navola]
--GO
--exec SAT_CON_ClaveProdServ ''
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'SAT_CON_ClaveProdServ'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE SAT_CON_ClaveProdServ;
END;
GO


CREATE procedure [dbo].SAT_CON_ClaveProdServ (  
@c_ClaveProdServ as varchar(10)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @c_ClaveProdServ=''   
Begin  
	set nocount off 
	Select c_ClaveProdServ,Descripcion from SAT_ClaveProdServ 
End   
Else  
Begin  
	set nocount off 
	Select c_ClaveProdServ,Descripcion from SAT_ClaveProdServ 
	where c_ClaveProdServ=@c_ClaveProdServ 
End  
  
End
--USE [Voalaft_Navola]
--GO
--exec SAT_CON_Impuesto ''
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'SAT_CON_Impuesto'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE SAT_CON_Impuesto;
END;
GO


CREATE procedure [dbo].SAT_CON_Impuesto (  
@c_Impuesto as varchar(10)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @c_Impuesto=''   
Begin  
	set nocount off 
	Select c_Impuesto,Descripcion,Retencion,Traslado,Local_o_federal from SAT_Impuesto 
End   
Else  
Begin  
	set nocount off 
	Select c_Impuesto,Descripcion,Retencion,Traslado,Local_o_federal from SAT_Impuesto 
	where c_Impuesto=@c_Impuesto 
End  
  
End
--USE [Voalaft_Navola]
--GO
--exec SAT_CON_ClaveUnidad ''
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'SAT_CON_ClaveUnidad'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE SAT_CON_ClaveUnidad;
END;
GO


CREATE procedure [dbo].SAT_CON_ClaveUnidad (  
@c_ClaveUnidad as varchar(10)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @c_ClaveUnidad=''   
Begin  
	set nocount off 
	Select c_ClaveUnidad,Nombre,Descripcion from SAT_ClaveUnidad 
End   
Else  
Begin  
	set nocount off 
	Select c_ClaveUnidad,Nombre,Descripcion from SAT_ClaveUnidad 
	where c_ClaveUnidad=@c_ClaveUnidad 
End  
  
End
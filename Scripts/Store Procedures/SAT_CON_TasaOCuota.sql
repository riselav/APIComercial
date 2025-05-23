--USE [Voalaft_Navola]
--GO
--exec SAT_CON_TasaOCuota ''
IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'SAT_CON_TasaOCuota'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE SAT_CON_TasaOCuota;
END;
GO


CREATE procedure [dbo].[SAT_CON_TasaOCuota] (  
@cValor as varchar(50)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @cValor=''   
Begin  
	set nocount off 
	Select [cValor], [Impuesto], [Factor], [Traslado], [Retencion] from SAT_TasaOCuota 
End   
Else  
Begin  
	set nocount off 
	Select top 1 [cValor], [Impuesto], [Factor], [Traslado], [Retencion] from SAT_TasaOCuota 
	where cValor=@cValor 
End  
  
End
--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatImpuestos'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatImpuestos;
END;
GO


CREATE procedure [dbo].[CAT_CON_CatImpuestos] (  
@nImpuesto as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nImpuesto=0   
Begin  
	set nocount off 
	Select nImpuesto, cDescripcion,nPorcentaje,nTipo,bImpuestoImporte,bExcento,c_Impuesto, bActivo,cTipoFactor,nTasaOCuota from Cat_Impuestos 
End   
Else  
Begin  
	set nocount off 
	Select nImpuesto, cDescripcion,nPorcentaje,nTipo,bImpuestoImporte,bExcento,c_Impuesto, bActivo,cTipoFactor,nTasaOCuota from Cat_Impuestos 
	where nImpuesto=@nImpuesto
End  
  
End
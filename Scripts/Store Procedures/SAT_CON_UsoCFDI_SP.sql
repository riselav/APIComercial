SP_ELIMINASTORE 'SAT_CON_UsoCFDI_SP'
GO
CREATE procedure [dbo].[SAT_CON_UsoCFDI_SP] (
	@cUsoCFDI varchar(10)=''
)  
AS   
--SAT_CON_UsoCFDI_SP 'D03'
	
SELECT UsoCFDI,Descripcion,AplicaPersonaFisica,AplicaPersonaMoral,FechaInicioVigencia,RegimenFiscalReceptor
FROM SAT_CatUsoCFDI (NOLOCK)
WHERE UsoCFDI= CASE WHEN @cUsoCFDI='' THEN UsoCFDI ELSE @cUsoCFDI END
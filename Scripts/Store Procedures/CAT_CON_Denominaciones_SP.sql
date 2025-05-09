SP_ELIMINASTORE 'CAT_CON_Denominaciones_SP'
GO
CREATE procedure [dbo].[CAT_CON_Denominaciones_SP] (
	@nDenominacion int=0
)  
AS   
--CAT_CON_Denominaciones_SP 2
	
SELECT nDenominacion,cDescripcion,nTipo,cImagen,nValor,bActivo,cUsuario_Registra,cMaquina_Registra
FROM CAT_Denominaciones (NOLOCK)
WHERE nDenominacion= CASE WHEN @nDenominacion=0 THEN nDenominacion ELSE @nDenominacion END
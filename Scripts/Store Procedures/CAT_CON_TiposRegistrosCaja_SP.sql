SP_ELIMINASTORE 'CAT_CON_TiposRegistrosCaja_SP'
GO
CREATE procedure [dbo].[CAT_CON_TiposRegistrosCaja_SP] (
	@nTipoRegistroCaja int=0
)  
AS   
--CAT_CON_TiposRegistrosCaja_SP 2
	
SELECT nTipoRegistroCaja,cDescripcion,nEfecto,bActivo,cUsuario_Registra,cMaquina_Registra
FROM CAT_TiposRegistroCaja (NOLOCK)
WHERE nTipoRegistroCaja= CASE WHEN @nTipoRegistroCaja=0 THEN nTipoRegistroCaja ELSE @nTipoRegistroCaja END
	AND bActivo=1
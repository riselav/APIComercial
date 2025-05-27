SP_ELIMINASTORE 'CAT_CON_FormasPago_SP'
GO
CREATE procedure [dbo].[CAT_CON_FormasPago_SP] (
	@nFormaPago int=0,
	@nTipoIngreso smallint=-1
)  
AS   
--CAT_CON_FormasPago_SP 2
--CAT_CON_FormasPago_SP 0,0
	
SELECT nFormaPago,cDescripcion,bIngreso,bActivo,cUsuario_Registra,cMaquina_Registra
FROM CAT_FormasPago (NOLOCK)
WHERE nFormaPago= CASE WHEN @nFormaPago=0 THEN nFormaPago ELSE @nFormaPago END
	AND bIngreso=CASE WHEN @nTipoIngreso=-1 THEN bIngreso ELSE @nTipoIngreso END
	AND bActivo=1
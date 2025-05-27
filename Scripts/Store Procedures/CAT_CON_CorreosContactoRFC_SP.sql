SP_ELIMINASTORE 'CAT_CON_CorreosContactoRFC_SP'
GO
CREATE procedure [dbo].[CAT_CON_CorreosContactoRFC_SP] (
	@nIDRFC bigint=0
)  
AS   
--CAT_CON_CorreosContactoRFC_SP 3
	
SELECT nIDRFC,nFolio,cCorreoElectronico,bActivo,
cUsuario_Registra,cMaquina_Registra
FROM CAT_CorreosContactoRFC (NOLOCK)
WHERE nIDRFC= CASE WHEN @nIDRFC=0 THEN nIDRFC ELSE @nIDRFC END
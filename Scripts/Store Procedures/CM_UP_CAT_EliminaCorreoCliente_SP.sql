SP_ELIMINASTORE 'CM_UP_CAT_EliminaCorreoCliente_SP'
GO
CREATE PROCEDURE CM_UP_CAT_EliminaCorreoCliente_SP (
	@nIDRFC bigint,  
	@cCorreoElectronico varchar(100)
)
AS
SET NOCOUNT ON

BEGIN TRY
	DELETE D
	FROM CAT_CorreosContactoRFC D (NOLOCK) --SET bActivo=0,cUsuario_Cancela=@cUsuarioCancela,dFecha_Cancela=GETDATE(),cMaquina_Cancela=@cMaquinaCancela,
	JOIN CAT_Clientes Cl (NOLOCK) ON Cl.nIDRFC=D.nIDRFC
	WHERE Cl.nIDRFC=@nIDRFC AND D.cCorreoElectronico=@cCorreoElectronico

	RETURN 1

	SET NOCOUNT OFF

END TRY
BEGIN CATCH
	RETURN 0

	SET NOCOUNT OFF
END CATCH
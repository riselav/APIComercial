SP_ELIMINASTORE 'CM_UP_CAT_EliminaContactoCliente_SP'
GO
CREATE PROCEDURE CM_UP_CAT_EliminaContactoCliente_SP (
	@nCliente bigint,  
	@nContacto int
)
AS
SET NOCOUNT ON

BEGIN TRY
	DELETE CAT_ClientesContactos --SET bActivo=0,cUsuario_Cancela=@cUsuarioCancela,dFecha_Cancela=GETDATE(),cMaquina_Cancela=@cMaquinaCancela,
	WHERE nCliente=@nCliente AND nContacto=@nContacto 

	RETURN 1

	SET NOCOUNT OFF

END TRY
BEGIN CATCH
	RETURN 0

	SET NOCOUNT OFF
END CATCH
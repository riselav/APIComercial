SP_ELIMINASTORE 'CM_UP_CAJ_CancelaMovimientoCaja_SP'
GO
CREATE PROCEDURE CM_UP_CAJ_CancelaMovimientoCaja_SP (
	@nIDRegistroCaja bigint,  
	@cUsuarioCancela varchar(100),
	@cMaquinaCancela varchar(100),
	@cUsuarioAutoriza varchar(100)
)
AS
SET NOCOUNT ON

UPDATE CAJ_MovimientosCaja SET bActivo=0,cUsuario_Cancela=@cUsuarioCancela,dFecha_Cancela=GETDATE(),cMaquina_Cancela=@cMaquinaCancela,
	   cUsuarioAutoriza_Cancelacion=@cUsuarioCancela
WHERE nIDRegistroCaja=@nIDRegistroCaja

SELECT 1 as Valor

SET NOCOUNT OFF
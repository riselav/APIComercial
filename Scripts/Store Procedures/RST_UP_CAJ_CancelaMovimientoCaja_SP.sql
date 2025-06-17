SP_ELIMINASTORE 'RST_UP_CAJ_CancelaMovimientoCaja_SP'
GO
CREATE PROCEDURE RST_UP_CAJ_CancelaMovimientoCaja_SP (
	@nIDApertura bigint,  
	@nTipoRegistroCaja int,
	@nConsecutivoRegistroCaja int,	
	@cUsuarioCancela varchar(100),
	@cMaquinaCancela varchar(100),
	@cUsuarioAutoriza varchar(100)
)
AS
SET NOCOUNT ON

UPDATE CAJ_MovimientosCaja SET bActivo=0,cUsuario_Cancela=@cUsuarioCancela,dFecha_Cancela=GETDATE(),cMaquina_Cancela=@cMaquinaCancela,
	   cUsuarioAutoriza_Cancelacion=@cUsuarioCancela
WHERE nIDApertura=@nIDApertura AND nTipoRegistroCaja=@nTipoRegistroCaja AND nConsecutivo=@nConsecutivoRegistroCaja

SELECT @nConsecutivoRegistroCaja

SET NOCOUNT OFF
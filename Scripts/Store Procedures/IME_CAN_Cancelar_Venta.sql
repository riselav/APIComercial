SP_ELIMINASTORE 'IME_CAN_Cancelar_Venta'
GO
CREATE PROCEDURE IME_CAN_Cancelar_Venta 
    @nVenta BIGINT,
	@nEmpleadoCancela INT,
    @nEmpleadoAutorizaCancelacion INT,
    @nMotivoCancelacion INT,
    @cObservacionesCancelacion VARCHAR(200),
	@cUsuario_Cancela VARCHAR(50),
	@cMaquina_Cancela VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @nIDRegistroCaja bigint;

	select @nIDRegistroCaja = nIDRegistroCaja from VTA_MovimientosVenta (nolock) where nVenta=@nVenta;

    update VTA_MovimientosVenta set bActivo = 0,
            nMotivoCancelacion = @nMotivoCancelacion,
            cObservacionesCancelacion = @cObservacionesCancelacion,
            nEmpleadoCancela = @nEmpleadoCancela,
			nEmpleadoAutorizaCancelacion=@nEmpleadoAutorizaCancelacion,
            dFecha_Cancela = GETDATE(),
			cUsuario_Cancela = @cUsuario_Cancela,
			cMaquina_Cancela = @cMaquina_Cancela
			where nVenta = @nVenta;

	update VTA_MovimientosVentaDetalle set bActivo = 0,
            dFecha_Cancela = GETDATE(),
			cUsuario_Cancela = @cUsuario_Cancela,
			cMaquina_Cancela = @cMaquina_Cancela
			where nVenta = @nVenta;

	if @nIDRegistroCaja is not null
	begin
		update CAJ_MovimientosCaja set bActivo = 0,
            dFecha_Cancela = GETDATE(),
			cUsuario_Cancela = @cUsuario_Cancela,
			cMaquina_Cancela = @cMaquina_Cancela
			where nIDRegistroCaja = @nIDRegistroCaja;

		update CAJ_DetalleMovimientosCaja set bActivo = 0,
            dFecha_Cancela = GETDATE(),
			cUsuario_Cancela = @cUsuario_Cancela,
			cMaquina_Cancela = @cMaquina_Cancela
			where nIDRegistroCaja = @nIDRegistroCaja;

		update CAJ_DetalleDenominacionMovimientosCaja set bActivo = 0,
            dFecha_Cancela = GETDATE(),
			cUsuario_Cancela = @cUsuario_Cancela,
			cMaquina_Cancela = @cMaquina_Cancela
			where nIDRegistroCaja = @nIDRegistroCaja;
		
	end
	
END
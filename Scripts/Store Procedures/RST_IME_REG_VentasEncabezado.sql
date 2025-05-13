SP_ELIMINASTORE 'RST_IME_REG_VentasEncabezado'
GO
CREATE PROCEDURE RST_IME_REG_VentasEncabezado 
    @nTipoRegistro TINYINT = 1, -- Venta
    @nTipoVenta TINYINT = NULL,
	@nSucursal INT,
    @nCaja INT,
    @nCliente BIGINT,
    @nIdLista INT = NULL,
    @nIDApertura BIGINT = NULL,
	@nFecha int,

	@nCotizacion BIGINT = NULL,
    @nVentaOrigenDevolucion BIGINT = NULL,
	@nEmpleado_Registra INT,

    @nSubtotal DECIMAL(18,4),
    @nImpuestoIVA DECIMAL(18,4),
	@nImpuestoIEPS DECIMAL(18,4) = NULL,
    @nImporteDescuento DECIMAL(18,2),
	@nPorcentajeDescuento DECIMAL(18,2) = NULL,
    @nTotal DECIMAL(18,2),

	@nIDRegistroCaja BIGINT = NULL,
	@nPagaCon decimal(18,2),
    @nCambio DECIMAL(18,2),
    
	@bFactura BIT,
	@nImporteFactura DECIMAL(18,2) = NULL,
    @nFacturaFinDia INT = NULL,
    @nFactura INT = NULL,

	@cComentarios VARCHAR(2000) = NULL,
    @cNombreCliente VARCHAR(100) = NULL,
    @nEmpleadoCancela INT = NULL,
    @nEmpleadoAutorizaCancelacion INT = NULL,
    @nMotivoCancelacion INT = NULL,
    @cObservacionesCancelacion VARCHAR(200) = NULL,

    @cUsuario_Registra VARCHAR(50),
    @cMaquina_Registra VARCHAR(50),
    @nVenta BIGINT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @NuevoConsecutivo INT;

    -- 1. Obtener nuevo consecutivo por sucursal
    IF @nTipoRegistro=1 OR @nTipoRegistro=2 --1=Venta 2=Venta Suspendida 
	BEGIN
		SELECT @NuevoConsecutivo = ISNULL(MAX(nConsecutivo), 0) + 1
		FROM VTA_MovimientosVenta (NOLOCK)
		WHERE nSucursal = @nSucursal 
			AND nCaja=@nCaja
			AND nTipoRegistro=@nTipoRegistro;

		-- 2. Construir el nVenta con formato:
		-- 1 + Sucursal (3) + Caja (2) + Consecutivo (9)
		SET @nVenta = CAST('1' +
                       RIGHT('000' + CAST(@nSucursal AS VARCHAR(3)), 3) +
                       RIGHT('000' + CAST(@nCaja AS VARCHAR(3)), 3) +
                       RIGHT('00000000' + CAST(@NuevoConsecutivo AS VARCHAR(8)), 8)
                  AS BIGINT);
	END
	ELSE
	BEGIN
		SELECT @NuevoConsecutivo = ISNULL(MAX(nConsecutivo), 0) + 1
		FROM VTA_MovimientosVenta (NOLOCK)
		WHERE nSucursal = @nSucursal 
			AND nTipoRegistro=@nTipoRegistro;
	
		-- 2. Construir el nVenta con formato:
		-- 1 + Sucursal (3) + Caja (2) + Consecutivo (9)
		SET @nVenta = CAST('1' +
                       RIGHT('00000' + CAST(@nSucursal AS VARCHAR(5)), 5) +
                       RIGHT('000000000' + CAST(@NuevoConsecutivo AS VARCHAR(9)), 9)
                  AS BIGINT);
	END
	
	DECLARE @dFecha datetime = (SELECT dbo.FechaNumero_Fn(@nFecha))

    -- 3. Insertar en la tabla
    INSERT INTO VTA_MovimientosVenta (
        nVenta,nTipoRegistro,nTipoVenta,nSucursal,nCaja,nCliente,nIdLista,nIDApertura,dFecha,nFecha,nConsecutivo,nCotizacion,nVentaOrigenDevolucion,
        nEmpleado_Registra,nSubtotal,nImpuestoIVA,nImpuestoIEPS,nImporteDescuento,nPorcentajeDescuento,nTotal,nIDRegistroCaja,nPagaCon,nCambio,bFactura,
        nImporteFactura,nFacturaFinDia,nFactura,cComentarios,cNombreCliente,nEmpleadoCancela,nEmpleadoAutorizaCancelacion,nMotivoCancelacion,cObservacionesCancelacion,
        bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
    VALUES (
        @nVenta,@nTipoRegistro,@nTipoVenta,@nSucursal,@nCaja,@nCliente,@nIdLista,@nIDApertura,@dFecha,@nFecha,@NuevoConsecutivo,@nCotizacion,@nVentaOrigenDevolucion,
        @nEmpleado_Registra,@nSubtotal,@nImpuestoIVA,@nImpuestoIEPS,@nImporteDescuento,@nPorcentajeDescuento,@nTotal,@nIDRegistroCaja,@nPagaCon,@nCambio,@bFactura,
        @nImporteFactura,@nFacturaFinDia,@nFactura,@cComentarios,@cNombreCliente,@nEmpleadoCancela,@nEmpleadoAutorizaCancelacion,@nMotivoCancelacion,@cObservacionesCancelacion,
        1,@cUsuario_Registra,@cMaquina_Registra,GETDATE()
    );
END
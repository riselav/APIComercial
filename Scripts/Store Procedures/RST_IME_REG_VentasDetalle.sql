SP_ELIMINASTORE 'RST_IME_REG_VentasDetalle'
GO
CREATE PROCEDURE RST_IME_REG_VentasDetalle
    @nVenta BIGINT,
    @nIDArticulo INT,
    @nCantidad DECIMAL(18,4),
    @nCantidadDevuelta DECIMAL(18,4)=NULL,
    @nPrecioUnitario DECIMAL(18,2),
    @nPrecioOriginal DECIMAL(18,2) = NULL,
    @nSubtotal DECIMAL(18,4),
    @nImpuestoIVA DECIMAL(18,4),
    @nIDImpuestoIVA INT,
    @nPorcentajeImpuestoIVA TINYINT,
    @nImpuestoIEPS DECIMAL(18,4) = NULL,
    @nIDImpuestoIEPS INT = NULL,
    @nPorcentajeImpuestoIEPS TINYINT = NULL,
    @nImporteDescuento DECIMAL(18,2) = NULL,
    @nPorcentajeDescuento DECIMAL(18,2) = NULL,
    @nTotal DECIMAL(18,2),
    @cComentarios VARCHAR(2000) = NULL,
    @nCostoUnitario DECIMAL(18,4) = NULL,
    @cUsuario_Registra VARCHAR(50),
    @cMaquina_Registra VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

	BEGIN TRY
		DECLARE @MaxRenglon SMALLINT,@nRenglon SMALLINT;

		-- 1. Obtener el próximo nRenglon para la venta
		SELECT @MaxRenglon = ISNULL(MAX(nRenglon), 0) + 1
		FROM VTA_MovimientosVentaDetalle (NOLOCK)
		WHERE nVenta = @nVenta;

		-- Asignamos el nRenglon
		SET @nRenglon = @MaxRenglon;

		-- 2. Insertar en la tabla
		INSERT INTO VTA_MovimientosVentaDetalle (
		nVenta,nRenglon,nIDArticulo,nCantidad,nCantidadDevuelta,nPrecioUnitario,nPrecioOriginal,nSubtotal,
		nImpuestoIVA,nIDImpuestoIVA,nPorcentajeImpuestoIVA,nImpuestoIEPS,nIDImpuestoIEPS,nPorcentajeImpuestoIEPS,
		nImporteDescuento,nPorcentajeDescuento,nTotal,cComentarios,nCostoUnitario,bActivo,
		cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
		VALUES (
		@nVenta,@nRenglon,@nIDArticulo,@nCantidad,@nCantidadDevuelta,@nPrecioUnitario,@nPrecioOriginal,@nSubtotal,
		@nImpuestoIVA,@nIDImpuestoIVA,@nPorcentajeImpuestoIVA,@nImpuestoIEPS,@nIDImpuestoIEPS,@nPorcentajeImpuestoIEPS,
		@nImporteDescuento,@nPorcentajeDescuento,@nTotal,@cComentarios,@nCostoUnitario,1, -- bActivo en 1 siempre (activo)
		@cUsuario_Registra,@cMaquina_Registra,GETDATE()
		);

		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN -1
	END CATCH
END
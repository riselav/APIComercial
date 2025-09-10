-- CM_CON_reporte_ventas_sp
CREATE OR ALTER PROCEDURE CM_CON_reporte_ventas_sp
    @nCajero INT = null
	,@bEstatus bit = null
	,@nTipoVenta int = null
	,@fechaInicio DATE = null
	,@fechaFin DATE = null
	,@cBusquedaGeneral VARCHAR(200) = null
AS
BEGIN
	
    SET @cBusquedaGeneral = UPPER('%' + ISNULL(@cBusquedaGeneral, '') + '%')

	select vta.nVenta, vta.nConsecutivo as folio,vta.nTipoRegistro, vta.dFecha_Registra as fechaHora, vta.nEmpleado_Registra, coalesce(emp.cnombre,'') as cajero, vta.nCliente, cli.cNombreCompleto, vta.nTotal as importe, vta.nFactura, vta.cComentarios, vta.bActivo  
	,vtaDet.nIDArticulo, art.cDescripcion, vtaDet.nCantidad, vtaDet.nPrecioUnitario, vtaDet.nTotal, vta.nIDRegistroCaja
	from VTA_MovimientosVenta vta (nolock)
	left join CAT_Empleados emp (nolock) on vta.nEmpleado_Registra = emp.nEmpleado
	left join CAT_Clientes cli (nolock) on vta.nCliente = cli.nCliente
	left join VTA_MovimientosVentaDetalle vtaDet (nolock) on vta.nVenta = vtaDet.nVenta
	left join CAT_Articulos art (nolock) on vtaDet.nIDArticulo=art.nIDArticulo  
	where  
		COALESCE(@nCajero,vta.nEmpleado_Registra) = vta.nEmpleado_Registra  
		and COALESCE(@bEstatus,vta.bActivo) = vta.bActivo
		and COALESCE(@nTipoVenta,vta.nTipoRegistro) = vta.nTipoRegistro
		and (@fechaInicio IS NULL OR CAST(vta.dFecha_Registra AS DATE) >= @fechaInicio)
		and (@fechaFin IS NULL OR CAST(vta.dFecha_Registra AS DATE) <= @fechaFin)
		and (@cBusquedaGeneral IS NULL OR 
			UPPER(vta.nConsecutivo) LIKE @cBusquedaGeneral OR
			UPPER(coalesce(emp.cnombre,'') +' '+ coalesce(emp.capellidopaterno,'') +' '+ coalesce(emp.capellidomaterno,'')) LIKE @cBusquedaGeneral OR
			UPPER(cli.cNombreCompleto) LIKE @cBusquedaGeneral OR
			UPPER(art.cDescripcion) LIKE @cBusquedaGeneral
		) 
	order by vta.nConsecutivo ASC, vta.nTipoRegistro

end

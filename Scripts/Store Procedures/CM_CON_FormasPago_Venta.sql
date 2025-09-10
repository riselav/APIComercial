SP_ELIMINASTORE 'CM_CON_FormasPago_Venta'
GO
CREATE procedure [dbo].CM_CON_FormasPago_Venta (
	@nVenta bigint=0	
)  
AS   
--CM_CON_FormasPago_Venta 100101000000061
	
SELECT fp.nFormaPago,fp.cDescripcion, mc.nImporte
FROM VTA_MovimientosVenta vta (nolock) 
inner join CAJ_DetalleMovimientosCaja mc (nolock) on vta.nIDRegistroCaja = mc.nIDRegistroCaja
inner join CAT_FormasPago fp (NOLOCK) on mc.nFormaPago=fp.nFormaPago
WHERE vta.nVenta=@nVenta
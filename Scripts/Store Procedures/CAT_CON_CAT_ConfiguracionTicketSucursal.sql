sp_eliminaStore 'CAT_CON_CAT_ConfiguracionTicketSucursal'
go

Create procedure CAT_CON_CAT_ConfiguracionTicketSucursal (@nSucursal int)    
AS    
-- EXEC CAT_CON_CAT_ConfiguracionTicketSucursal 3
Begin    
	 SELECT nSucursal,
	 cEncabezado1,
	 cEncabezado2,
	 cEncabezado3,
	 cEncabezado4,
	 cEncabezado5,
	 cEncabezado6,
	 cEncabezado7,
	 cEncabezado8,
	 cEncabezado9
	FROM CAT_EncabezadosSucursal (NOLOCK) 
	WHERE nSucursal=@nSucursal

	SELECT nSucursal,
	cPieTicket1,
	cPieTicket2,
	cPieTicket3,
	cPieTicket4,
	cPieTicket5,
	cPieTicket6,
	cPieTicket7,
	cPieTicket8,
	cPieTicket9
	FROM CAT_PieTicketSucursal (NOLOCK)
	WHERE nSucursal=@nSucursal
End
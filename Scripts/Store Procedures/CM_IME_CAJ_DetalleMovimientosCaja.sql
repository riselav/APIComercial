
SP_ELIMINASTORE 'CM_IME_CAJ_DetalleMovimientosCaja'
GO

CREATE PROC CM_IME_CAJ_DetalleMovimientosCaja( 
	@nIDRegistroCaja bigint,
	@nRenglon int,
	@nFormaPago int,
	@nImporte decimal(18, 4),
	@bActivo bit,
	@cUsuario_Registra varchar(50) ,
	@cMaquina_Registra varchar(50) ,
	@dFecha_Registra datetime ,
	@nOrden bigint ,
	@nCuenta bigint ,
	@cReferencia varchar(20) = null,
	@cReferenciaCuponVale varchar(50) = null,
	@nImporteFacturado decimal(18, 2) = null,
	@nImportePropina decimal(18, 2) = null,
	@nCliente int = null,
	@nEmpleado int = null,
	@nPropina decimal(18,2) = null,
	@nPagaCon Decimal (18,2)=0,
	@nCambio Decimal(18,2)=0
)          
AS  

BEGIN TRY          
 SET NOCOUNT ON          
          
  DECLARE @EXISTE INT = COALESCE( (SELECT COUNT(NORDEN)  FROM CAJ_DetalleMovimientosCaja WHERE nIDRegistroCaja = @nIDRegistroCaja 
			AND	nRenglon = @nRenglon AND nFormaPago = @nFormaPago ),0)  

 IF @EXISTE = 0        
   BEGIN   

   INSERT INTO [dbo].[CAJ_DetalleMovimientosCaja]
           ([nIDRegistroCaja],[nRenglon],[nFormaPago],[nImporte],[bActivo],[cUsuario_Registra],[cMaquina_Registra],[dFecha_Registra]
				,[nOrden],[nCuenta],[cReferencia],[cReferenciaCuponVale],[nImporteFacturado],[nImportePropina]
           ,[nCliente],[nEmpleado],[nPropina], [nPagaCon], [nCambio])

		  SELECT @nIDRegistroCaja,@nRenglon,@nFormaPago,@nImporte,@bActivo,@cUsuario_Registra,@cMaquina_Registra,@dFecha_Registra
				,@nOrden,@nCuenta,@cReferencia,@cReferenciaCuponVale,@nImporteFacturado,@nImportePropina
           ,@nCliente,@nEmpleado,@nPropina, @nPagaCon, @nCambio
     END        
  
 SET NOCOUNT OFF          
           
 RETURN 1          
END TRY          
BEGIN CATCH          
 SET NOCOUNT OFF           
          
 RETURN -1          
END CATCH  


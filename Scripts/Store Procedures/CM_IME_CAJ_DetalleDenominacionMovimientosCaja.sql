SP_ELIMINASTORE 'CM_IME_CAJ_DetalleDenominacionMovimientosCaja'
GO
CREATE PROCEDURE CM_IME_CAJ_DetalleDenominacionMovimientosCaja (        
	@nFolio bigint,  
	@nRenglon int,
	@nDenominacion int,	
	@nValor decimal(18,4),
	@nCantidad int,
	@nImporte decimal(18,4),	
	@cUsuario varchar(100),
	@cMaquina varchar(100)
)        
AS         
BEGIN
	BEGIN TRY		
		INSERT INTO CAJ_DetalleDenominacionMovimientosCaja (
		nIDRegistroCaja,nRenglon,nDenominacion,nValor,nCantidad,nImporte,
		bActivo, cUsuario_Registra, cMaquina_Registra,dFecha_Registra)

		SELECT @nFolio,@nRenglon,@nDenominacion,@nValor,@nCantidad,@nImporte,	
		1 as bActivo, @cUsuario,@cMaquina,GETDATE()
        
		RETURN 1
	END TRY
	BEGIN CATCH
		RETURN 0
	END CATCH
END
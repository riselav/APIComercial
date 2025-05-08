SP_ELIMINASTORE 'RST_IME_CAT_PrecioArticulo'
GO

--SELECT * FROM CAT_Precios (NOLOCK)
CREATE PROC RST_IME_CAT_PrecioArticulo (
	@nIdSucursal int,
	@nIdArticulo int,
	@nPrecio decimal(18,2),
	@cUsuario as Varchar(50),
	@cNombreMaquina as Varchar(50)=NULL,
	@nIdListaPrecio int=1,
	@nIdMoneda int=1
)
AS
-- SELECT * FROM CAT_Sucursales (NOLOCK)

-- EXEC RST_IME_CAT_PrecioArticulo 1,1,25.2,'WEB'

DECLARE @nFolioSig_Precio as INT

IF @cNombreMaquina IS NULL SET @cNombreMaquina=(SELECT HOST_NAME())

IF @nPrecio>0
BEGIN
	DECLARE @nPrecioActual decimal(18,4)=(SELECT nPrecio FROM CAT_Precios (NOLOCK) 
											WHERE nIdSucursal =@nIdSucursal AND nIdArticulo=@nIdArticulo AND bActivo=1 
											AND nIdLista=@nIdListaPrecio AND nIdMoneda=@nIdMoneda)
		
	IF @nPrecioActual IS NOT NULL
	BEGIN
		IF @nPrecio<>@nPrecioActual
		BEGIN
			SET @nFolioSig_Precio=  (SELECT ISNULL(MAX(nFolio),0) FROM CAT_HistoricoPrecios(NOLOCK)) + 1   

			INSERT INTO CAT_HistoricoPrecios(nFolio,nIDArticulo,nIdLista,nIdSucursal,nIdMoneda,nPrecio,
									bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
			SELECT @nFolioSig_Precio,@nIdArticulo,@nIdListaPrecio as nIdLista,@nIdSucursal,@nIdMoneda as nIdMoneda,@nPrecioActual,
					1,@cUsuario,@cNombreMaquina,GETDATE()

			UPDATE CAT_Precios SET nPrecio=@nPrecio,
			cUsuario_Modifica= @cUsuario,        
			cMaquina_Modifica= @cNombreMaquina,        
			dFecha_Modifica = GETDATE ()     
			WHERE nIdSucursal =@nIdSucursal AND bActivo=1 
			AND nIdLista=1 AND nIdMoneda=1
		END
	END
	ELSE
	BEGIN
		SET @nFolioSig_Precio=  (SELECT ISNULL(MAX(nFolio),0) FROM CAT_Precios(NOLOCK)) + 1 

		INSERT INTO CAT_Precios(nFolio,nIDArticulo,nIdLista,nIdSucursal,nIdMoneda,nPrecio,
							bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
		SELECT @nFolioSig_Precio,@nIdArticulo,@nIdListaPrecio as nIdLista,@nIdSucursal,@nIdMoneda as nIdMoneda,@nPrecio,
				1,@cUsuario,@cNombreMaquina,GETDATE()
	END
END

RETURN 1
SP_ELIMINASTORE 'RST_IME_CAT_Articulo'
GO

--SELECT * FROM CAT_Articulos (NOLOCK)
CREATE PROC RST_IME_CAT_Articulo (        
	@nFolio as int,
	@cClave varchar (20),
	@cDescripcion Varchar(100),        
	@nUnidad as Int,
	@cPresentacion varchar(50),
	@nLinea int,
	@nSublinea int,
	@cClaveSAT varchar(50)=NULL,
	@bInsumoFinal bit,
	@nIdProductoBase int,
	@nIdUnidadRelacional int,
	@nEquivalencia decimal(18,6),
	@nTipoArticulo smallint,
	@bManejaInventario bit,
	@bActivo as bit,        
	@cUsuario as Varchar(50),
	@nPrecioGeneral decimal(18,2)=0,
	@nIdImpuestoIVA int,
	@nIdImpuestoIEPS int=NULL,
	@bDesglosaImpuestoIEPS bit=0,
	@nImporteImpuestoIEPS decimal(18,4)=NULL,
	@cNombreMaquina as Varchar(50)=NULL,
	@nMarca int =NULL,
	@nIdListaPrecio int=1,
	@nIdMoneda int=1,
	@bLote bit=0,
	@bSerie bit=0,
	@bPedimento bit=0,
	@bProductoBase bit=0
)        
AS         
BEGIN        
	set nocount on
	-- EXEC RST_IME_CAT_Articulo 1,'ART11','LECHE DE LITRO NUTRILECHE',3,'PIEZA',1,1,NULL,0,2,7,1.0,1,1,1,'WEB',22.2,2

	IF @cNombreMaquina IS NULL SET @cNombreMaquina=(SELECT HOST_NAME())
	--=================================================================================================================        
	-- Si el Folio es cero, indica que es un nuevo registro,        
	--=================================================================================================================     
	
	DECLARE @nFolioSig_Precio as INT

	IF @nFolio =0         
	BEGIN        
		 DECLARE @nFolioSig as INT =  (SELECT ISNULL(MAX(nIDArticulo),0) FROM CAT_Articulos(NOLOCK)) + 1        
        
		 INSERT INTO CAT_Articulos (nIDArticulo,cClave,cDescripcion, nUnidad,cPresentacion,
		 nLinea,nSublinea,nMarca,cClaveSAT,nTipoArticulo,
		 bLote,bSerie,bPedimento,
		 bInsumoFinal,bProductoBase,
		 nIdProductoBase,nIdUnidadRelacional,nEquivalencia,bManejaInventario,
		 bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)        
		 SELECT @nFolioSig,@cClave, @cDescripcion,@nUnidad,@cPresentacion,
		 @nLinea,@nSublinea,@nMarca,@cClaveSAT,@nTipoArticulo,
		 @bLote,@bSerie,@bPedimento,
		 @bInsumoFinal,@bProductoBase,
		 @nIdProductoBase,@nIdUnidadRelacional,@nEquivalencia,@bManejaInventario,
		 1, @cUsuario, @cNombreMaquina, GETDATE()         
        
		 INSERT INTO CAT_ImpuestosArticulos(nIDArticulo,nImpuesto,nImporte,bDesglosa,
		 bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
		 SELECT @nFolioSig,@nIdImpuestoIVA,0,0,
		 1,@cUsuario,@cNombreMaquina,GETDATE()

		 IF @nIdImpuestoIEPS IS NOT NULL
		 BEGIN
			 INSERT INTO CAT_ImpuestosArticulos(nIDArticulo,nImpuesto,nImporte,bDesglosa,
			 bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
			 SELECT @nFolioSig,@nIdImpuestoIEPS,@nImporteImpuestoIEPS,@bDesglosaImpuestoIEPS,
			 1,@cUsuario,@cNombreMaquina,GETDATE()
		 END

		 IF @nPrecioGeneral>0
		 BEGIN
			SET @nFolioSig_Precio=  (SELECT ISNULL(MAX(nFolio),0) FROM CAT_Precios(NOLOCK)) + 1   

			INSERT INTO CAT_Precios(nFolio,nIDArticulo,nIdLista,nIdSucursal,nIdMoneda,nPrecio,
									bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
			SELECT @nFolioSig_Precio,@nFolioSig,@nIdListaPrecio as nIdLista,NULL,@nIdMoneda as nIdMoneda,@nPrecioGeneral,
					1,@cUsuario,@cNombreMaquina,GETDATE()
		 END
		 set nocount off
		 RETURN @nFolioSig
	END         
	ELSE        
	BEGIN        
        
	--=================================================================================================================        
	-- Si el Folio es mayor a cero, se valora el campo bActivo:        
	--    Si el valor es 1, entonces indica que es una modificación        
	--    Si el valor es 0, entonces se trata de una cancelacion        
	--=================================================================================================================        
	
	-- Actualiza el registro indicado        
        
	 IF @bActivo =1          
	 BEGIN        
		UPDATE R SET cDescripcion= @cDescripcion,       
		nTipoArticulo = @nTipoArticulo,         
		bactivo= @bActivo,        
		cUsuario_Modifica= @cUsuario,        
		cMaquina_Modifica= @cNombreMaquina,        
		dFecha_Modifica = GETDATE ()        
		FROM CAT_Articulos (NOLOCK) as R        
		WHERE nIDArticulo = @nFolio        
        
		DELETE CAT_ImpuestosArticulos WHERE nIDArticulo=@nFolio

		INSERT INTO CAT_ImpuestosArticulos(nIDArticulo,nImpuesto,nImporte,bDesglosa,
		 bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
		 SELECT @nFolio,@nIdImpuestoIVA,0,0,
		 1,@cUsuario,@cNombreMaquina,GETDATE()

		 IF @nIdImpuestoIEPS IS NOT NULL
		 BEGIN
			 INSERT INTO CAT_ImpuestosArticulos(nIDArticulo,nImpuesto,nImporte,bDesglosa,
			 bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
			 SELECT @nFolio,@nIdImpuestoIEPS,@nImporteImpuestoIEPS,@bDesglosaImpuestoIEPS,
			 1,@cUsuario,@cNombreMaquina,GETDATE()
		 END
		 
		 IF @nPrecioGeneral>0
		 BEGIN
			DECLARE @nPrecioActual decimal(18,4)=(SELECT top 1 nPrecio FROM CAT_Precios (NOLOCK) 
												  WHERE nIdSucursal IS NULL AND bActivo=1 
												  AND nIdLista=@nIdListaPrecio AND nIdMoneda=@nIdMoneda)
		
			IF @nPrecioActual IS NOT NULL
			BEGIN
				IF @nPrecioGeneral<>@nPrecioActual
				BEGIN
					SET @nFolioSig_Precio=  (SELECT ISNULL(MAX(nFolio),0) FROM CAT_HistoricoPrecios(NOLOCK)) + 1   

					INSERT INTO CAT_HistoricoPrecios(nFolio,nIDArticulo,nIdLista,nIdSucursal,nIdMoneda,nPrecio,
											bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
					SELECT @nFolioSig_Precio,@nFolio,@nIdListaPrecio as nIdLista,NULL,@nIdMoneda as nIdMoneda,@nPrecioActual,
							1,@cUsuario,@cNombreMaquina,GETDATE()

					UPDATE CAT_Precios SET nPrecio=@nPrecioGeneral,
					cUsuario_Modifica= @cUsuario,        
					cMaquina_Modifica= @cNombreMaquina,        
					dFecha_Modifica = GETDATE ()     
					WHERE nIdSucursal IS NULL AND bActivo=1 
					AND nIdLista=1 AND nIdMoneda=1
				END
			END
			ELSE
			BEGIN
				SET @nFolioSig_Precio=  (SELECT ISNULL(MAX(nFolio),0) FROM CAT_Precios(NOLOCK)) + 1 

				INSERT INTO CAT_Precios(nFolio,nIDArticulo,nIdLista,nIdSucursal,nIdMoneda,nPrecio,
									bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
				SELECT @nFolioSig_Precio,@nFolio,@nIdListaPrecio as nIdLista,NULL,@nIdMoneda as nIdMoneda,@nPrecioGeneral,
						1,@cUsuario,@cNombreMaquina,GETDATE()
			END
		 END
		set nocount off
		RETURN @nFolio        
	 END         
     
	 
	-- Cancela el registro indicado        
         
	 UPDATE R Set bActivo= @bActivo,         
		  cUsuario_Cancela= @cUsuario,        
		  cMaquina_Cancela= @cNombreMaquina,        
		  dFecha_Cancela = GETDATE()        
	  FROM CAT_Articulos (NOLOCK) as R        
	  WHERE nIDArticulo = @nFolio        
      set nocount off
	  RETURN @nFolio        
	END        
END 
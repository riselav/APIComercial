SP_ELIMINASTORE 'RST_IME_CAT_ProductosBase'
GO

--SELECT * FROM CAT_ProductosBase (NOLOCK)
CREATE PROC RST_IME_CAT_ProductosBase (        
	@nFolio as int,          
	@cDescripcion Varchar(100),        
	@nTipoUnidad as TinyInt,        
	@bActivo as bit,        
	@cUsuario as Varchar(50),        
	@cNombreMaquina as Varchar(50)=NULL
)        
AS         
BEGIN        

	-- EXEC RST_IME_CAT_ProductosBase 2,'LECHE',2,1,'WEB'

	IF @cNombreMaquina IS NULL SET @cNombreMaquina=(SELECT HOST_NAME())
	--=================================================================================================================        
	-- Si el Folio es cero, indica que es un nuevo registro,        
	--=================================================================================================================        
	IF @nFolio =0         
	BEGIN        
	 DECLARE @nFolioSig as INT =  (SELECT ISNULL(MAX(nProductoBase),0) FROM CAT_ProductosBase(NOLOCK)) + 1        
        
	 INSERT INTO CAT_ProductosBase (nProductoBase,cDescripcion, nTipoUnidad, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)        
	 SELECT @nFolioSig, @cDescripcion,@nTipoUnidad, 1, @cUsuario, @cNombreMaquina, GETDATE()         
        
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
		--nTipoUnidad = @nTipoUnidad,         
		bactivo= @bActivo,        
		cUsuario_Modifica= @cUsuario,        
		cMaquina_Modifica= @cNombreMaquina,        
		dFecha_Modifica = getdate ()        
		FROM CAT_ProductosBase (NOLOCK) as R        
		WHERE nProductoBase = @nFolio        
        
		RETURN @nFolio        
	 END         
        
	-- Cancela el registro indicado        
         
	 UPDATE R Set bActivo= @bActivo,         
		  cUsuario_Cancela= @cUsuario,        
		  cMaquina_Cancela= @cNombreMaquina,        
		  dFecha_Cancela = GETDATE()        
	  FROM CAT_ProductosBase (NOLOCK) as R        
	  WHERE nProductoBase = @nFolio        
        
	  RETURN @nFolio        
	END        
END 
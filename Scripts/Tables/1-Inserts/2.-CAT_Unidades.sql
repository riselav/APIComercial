-- SELECT * FROM CAT_Unidades (NOLOCK)

DECLARE @nCatalogo int=(SELECT MAX(nUnidad) FROM CAT_Unidades (NOLOCK))

IF @nCatalogo IS NULL SET @nCatalogo=0

IF NOT EXISTS (SELECT 1 FROM CAT_Unidades (NOLOCK) WHERE nUnidad=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Unidades(nUnidad,cDescripcion,cAbreviatura,nTipoUnidad,cClaveSAT,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT @nCatalogo,'KILOGRAMO','KG', 1,'KGM', 
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_Unidades (NOLOCK) WHERE nUnidad=2)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Unidades(nUnidad,cDescripcion,cAbreviatura,nTipoUnidad,cClaveSAT,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT @nCatalogo,'LITRO','LT', 2,'LTR', 
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_Unidades (NOLOCK) WHERE nUnidad=3)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Unidades(nUnidad,cDescripcion,cAbreviatura,nTipoUnidad,cClaveSAT,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT @nCatalogo,'PIEZA','PZA', 3,'H87', 
	1,'sa',HOST_NAME(),GETDATE()
END
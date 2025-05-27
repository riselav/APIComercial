IF NOT EXISTS (SELECT 1 FROM CAT_ListasPrecio (NOLOCK) WHERE nIdLista=1)
BEGIN
	INSERT INTO CAT_ListasPrecio(nIdLista,cDescripcion,	bActivo,
	cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 1,'PÚBLICO GENERAL', 1,
	'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_ListasPrecio (NOLOCK) WHERE nIdLista=2)
BEGIN
	INSERT INTO CAT_ListasPrecio(nIdLista,cDescripcion,	bActivo,
	cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 2,'MAYORISTA', 0,
	'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_ListasPrecio (NOLOCK) WHERE nIdLista=3)
BEGIN
	INSERT INTO CAT_ListasPrecio(nIdLista,cDescripcion,	bActivo,
	cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 3,'DISTRIBUIDOR', 0,
	'sa',HOST_NAME(),GETDATE()
END

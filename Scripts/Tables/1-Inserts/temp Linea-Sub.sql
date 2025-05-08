IF NOT EXISTS (SELECT 1 FROM CAT_Lineas (NOLOCK) WHERE nLinea=1)
BEGIN
	INSERT INTO CAT_Lineas(nLinea,cDescripcion,	bActivo,
	cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 1,'LINEA DEFAULT', 1,
	'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_SubLineas (NOLOCK) WHERE nLinea=1 AND nSublinea=1)
BEGIN
	INSERT INTO CAT_SubLineas(nSublinea,nLinea,cDescripcion,bActivo,
	cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 1,1,'SUBLINEA DEFAULT', 1,
	'sa',HOST_NAME(),GETDATE()
END
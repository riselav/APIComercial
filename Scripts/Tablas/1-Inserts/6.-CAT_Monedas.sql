IF NOT EXISTS (SELECT 1 FROM CAT_Monedas (NOLOCK) WHERE nMoneda=1)
BEGIN
	INSERT INTO CAT_Monedas(nMoneda,cDescripcion,cSigno,bMonedaBase,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 1,'PESO','$',1,
	1,'WEB',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_Monedas (NOLOCK) WHERE nMoneda=2)
BEGIN
	INSERT INTO CAT_Monedas(nMoneda,cDescripcion,cSigno,bMonedaBase,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 2,'DOLAR','',0,
	1,'WEB',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_Monedas (NOLOCK) WHERE nMoneda=3)
BEGIN
	INSERT INTO CAT_Monedas(nMoneda,cDescripcion,cSigno,bMonedaBase,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 3,'EURO','',0,
	1,'WEB',HOST_NAME(),GETDATE()
END

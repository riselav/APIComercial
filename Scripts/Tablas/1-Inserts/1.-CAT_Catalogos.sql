DECLARE @nCatalogo int=(SELECT MAX(nCatalogo) FROM CAT_Catalogos (NOLOCK))

IF @nCatalogo IS NULL SET @nCatalogo=0

IF NOT EXISTS (SELECT 1 FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TiposUnidad' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TiposUnidad',1,'PESO',1
END

IF NOT EXISTS (SELECT 1 FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TiposUnidad' AND nCodigo=2)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TiposUnidad',2,'LÍQUIDO',1
END

IF NOT EXISTS (SELECT 1 FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TiposUnidad' AND nCodigo=3)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TiposUnidad',3,'PIEZA',1
END

IF NOT EXISTS (SELECT 1 FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TiposArticulo' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TiposArticulo',1,'INDIVIDUAL',1
END

IF NOT EXISTS (SELECT 1 FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TiposArticulo' AND nCodigo=2)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TiposArticulo',2,'KIT',1
END

IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TipoContactoCliente' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TipoContactoCliente',1,'CORREO',1
END



IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TipoContactoCliente' AND nCodigo=2)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TipoContactoCliente',2,'LLAMADA',1
END


IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TipoContactoCliente' AND nCodigo=3)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TipoContactoCliente',3,'MENSAJE',1
END


IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TipoContactoCliente' AND nCodigo=4)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TipoContactoCliente',4,'OFICINA',1
END

IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_RESPUESTASINO' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_RESPUESTASINO',1,'SI',1
END


IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_RESPUESTASINO' AND nCodigo=0)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_RESPUESTASINO',0,'NO',1
END


IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_EFECTOSINVENTARIOS' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_EFECTOSINVENTARIOS',1,'ENTRADA',1
END



IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_EFECTOSINVENTARIOS' AND nCodigo=-1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_EFECTOSINVENTARIOS',-1,'SALIDA',1
END


IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TIPOSINVOLUCRADOS' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TIPOSINVOLUCRADOS',1,'PROVEEDOR',1
END


IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TIPOSINVOLUCRADOS' AND nCodigo=2)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TIPOSINVOLUCRADOS',2,'EMPLEADO',1
END



IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TIPOSINVOLUCRADOS' AND nCodigo=3)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TIPOSINVOLUCRADOS',3,'CLIENTE',1
END

IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TIPOSDOCUMENTOS' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TIPOSDOCUMENTOS',1,'COTIZACION',1
END

IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='CAT_TIPOSDOCUMENTOS' AND nCodigo=2)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'CAT_TIPOSDOCUMENTOS',2,'VENTA',1
END


IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='Cat_TipoPersona' AND nCodigo=1)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'Cat_TipoPersona',1,'FISICA',1
END

IF NOT EXISTS (SELECT * FROM CAT_Catalogos (NOLOCK) WHERE cNombre='Cat_TipoPersona' AND nCodigo=2)
BEGIN
	SET @nCatalogo=@nCatalogo+1

	INSERT INTO CAT_Catalogos
	SELECT @nCatalogo,'Cat_TipoPersona',2,'MORAL',1
END
-- select * from CAT_Catalogos where cNombre= 'Cat_TipoPersona'
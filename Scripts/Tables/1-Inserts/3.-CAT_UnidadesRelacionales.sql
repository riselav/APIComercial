IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=1)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 1,'KILOGRAMO', 1 as nTipoUnidad,1 as bEsBase,1 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=2)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 2,'GRAMO', 1 as nTipoUnidad,0 as bEsBase,0.001 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=3)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 3,'MILIGRAMO', 1 as nTipoUnidad,0 as bEsBase,0.000001 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=4)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 4,'LIBRA', 1 as nTipoUnidad,0 as bEsBase,0.45359237 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=5)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 5,'DECIGRAMO', 1 as nTipoUnidad,0 as bEsBase,0.0001 as nEquivalencia,
	0,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=6)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 6,'CENTIGRAMO', 1 as nTipoUnidad,0 as bEsBase,0.00001 as nEquivalencia,
	0,'sa',HOST_NAME(),GETDATE()
END
--

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=7)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 7,'LITRO', 2 as nTipoUnidad,1 as bEsBase,1 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=8)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 8,'MILILITRO', 2 as nTipoUnidad,0 as bEsBase,0.001 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=9)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 9,'GALON', 2 as nTipoUnidad,0 as bEsBase,3.78541178 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=10)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 10,'DECILITRO', 2 as nTipoUnidad,0 as bEsBase,0.1 as nEquivalencia,
	0,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=11)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 11,'CENTILITRO', 2 as nTipoUnidad,0 as bEsBase,0.01 as nEquivalencia,
	0,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=12)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 12,'DECALITRO', 2 as nTipoUnidad,0 as bEsBase,10 as nEquivalencia,
	0,'sa',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=13)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 13,'HECTOLITRO', 2 as nTipoUnidad,0 as bEsBase,100 as nEquivalencia,
	0,'sa',HOST_NAME(),GETDATE()
END
IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=14)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 14,'KILOLITRO', 2 as nTipoUnidad,0 as bEsBase,1000 as nEquivalencia,
	0,'sa',HOST_NAME(),GETDATE()
END

--
IF NOT EXISTS (SELECT 1 FROM CAT_UnidadesRelacionales (NOLOCK) WHERE nUnidadRelacional=15)
BEGIN
	INSERT INTO CAT_UnidadesRelacionales(nUnidadRelacional,cDescripcion,nTipoUnidad,bEsBase,nEquivalencia,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 15,'PIEZA', 3 as nTipoUnidad,1 as bEsBase,1 as nEquivalencia,
	1,'sa',HOST_NAME(),GETDATE()
END
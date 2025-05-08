IF NOT EXISTS (SELECT 1 FROM CAT_Impuestos (NOLOCK) WHERE nImpuesto=1)
BEGIN
	INSERT INTO CAT_Impuestos(nImpuesto,cDescripcion,nPorcentaje,
	nTipo,bImpuestoImporte,bExcento,c_Impuesto,cTipoFactor,nTasaOCuota,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 1,'IVA 16 %',16,
	1 as nTipo,0,0,'002','Tasa',0.160000,
	1 as bActivo,'WEB',HOST_NAME(),GETDATE()
END

IF NOT EXISTS (SELECT 1 FROM CAT_Impuestos (NOLOCK) WHERE nImpuesto=2)
BEGIN
	INSERT INTO CAT_Impuestos(nImpuesto,cDescripcion,nPorcentaje,
	nTipo,bImpuestoImporte,bExcento,c_Impuesto,cTipoFactor,nTasaOCuota,
	bActivo,cUsuario_Registra,cMaquina_Registra,dFecha_Registra)
	SELECT 2,'IVA 0 %',0,
	1 as nTipo,0,0,'002','Tasa',0.000000,
	1 as bActivo,'WEB',HOST_NAME(),GETDATE()
END 
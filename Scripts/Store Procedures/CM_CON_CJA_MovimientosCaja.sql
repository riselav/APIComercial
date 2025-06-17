SP_ELIMINASTORE 'CM_CON_CJA_MovimientosCaja'
GO
CREATE PROCEDURE CM_CON_CJA_MovimientosCaja (@nSucursal int,@nCaja int,@nTipoRegistroCaja int)  
AS  
BEGIN  
 -- CM_CON_CJA_MovimientosCaja 1,2,4

 DECLARE @nIDApertura bigint=(SELECT nIDApertura FROM CAJ_RegistrosAperturaCaja (NOLOCK) WHERE nIDSucursal= @nSucursal AND nIDCaja=@nCaja AND nEstatus=1)

 DECLARE @MovimientosCaja as table(
	nFolio int,
	nIDRegistroCaja bigint,
	cConceptoCaja varchar(200),
	nImporte decimal(18,2),
	dFecha date,
	cUsuario varchar(100),
	cEmpleado varchar(500),
	cHora varchar(30),
	cComentarios varchar(5000),
	nRenglon int
 )

 IF @nIDApertura IS NOT NULL AND @nSucursal>0 AND @nCaja >0
 BEGIN
	INSERT INTO @MovimientosCaja
	SELECT MC.nConsecutivo, MC.nIDRegistroCaja,CC.cDescripcion as cConceptoCaja ,MC.nImporte, MC.dfecha ,
	MC.cUsuario_Registra, RTRIM(LTRIM(E.cNombre+ ' ' + ISNULL(E.cApellidoPaterno,'') + ' ' + ISNULL(E.cApellidoMaterno,''))) as cEmpleado, 
	CONVERT(varchar(10), MC.dFecha_Registra,108),ISNULL(MC.cObservaciones,'') as cObservaciones,
	ROW_NUMBER()OVER (PARTITION BY MC.nSucursal ORDER BY MC.nIDRegistroCaja )
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	LEFT JOIN CAT_Empleados E (NOLOCK) ON E.nEmpleado=MC.nEmpleadoInvolucrado
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.nIDApertura= @nIDApertura AND MC.bActivo=1
	AND MC.nTipoRegistroCaja=@nTipoRegistroCaja 
	ORDER BY MC.dFecha_Registra

	UPDATE @MovimientosCaja SET nFolio=nRenglon WHERE nFolio IS NULL
 END
 SELECT * FROM @MovimientosCaja
END 
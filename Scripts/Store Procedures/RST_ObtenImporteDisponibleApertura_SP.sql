SP_ELIMINASTORE 'RST_ObtenImporteDisponibleApertura_SP'
GO
CREATE PROC RST_ObtenImporteDisponibleApertura_SP(
	@nIDApertura bigint
)
AS
-- RST_ObtenImporteDisponibleApertura_SP 10000100000033

DECLARE @Datos as table(
	nFormaPago int,
	cFormaPago varchar(200),
	nImporteSistema decimal(18,4),
	bEfectivo bit
)

IF @nIDApertura IS NOT NULL
BEGIN
	--Pago de Órdenes
	INSERT INTO @Datos
	SELECT FP.nFormaPago,FP.cDescripcion as cFormaPago,SUM(MD.nImporte) as nImporteSistema,
	CASE WHEN FP.nFormaPago=1 THEN 1 ELSE 0 END as bEfectivo
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN CAJ_DetalleMovimientosCaja MD (NOLOCK) ON MC.nIDRegistroCaja=MD.nIDRegistroCaja
	JOIN CAT_FormasPago FP (NOLOCK) ON FP.nFormaPago=MD.nFormaPago and FP.bActivo =1 
	WHERE MC.nIDApertura=@nIDApertura AND MC.bActivo=1 AND FP.bActivo=1
	     AND MC.nTipoRegistroCaja =5 -- <== Pago=5
		 AND FP.nFormaPago=1 --<== Efectivo=1
	GROUP BY FP.nFormaPago,FP.cDescripcion

	---- Pago de Embarques
	--INSERT INTO @Datos
	--SELECT FP.nFormaPago,FP.cDescripcion as cFormaPago,SUM(MC.nImporte*MC.nEfecto) as nImporteSistema,
	--CASE WHEN FP.nFormaPago=1 THEN 1 ELSE 0 END as bEfectivo
	--FROM CAJ_MovimientosCaja MC (NOLOCK)
	--JOIN REG_EmbarqueOrdenes EO (NOLOCK) ON EO.nIDRegistroCaja=MC.nIDRegistroCaja
	----JOIN CAT_TiposRegistroCaja TE (NOLOCK) ON TE.nTipoRegistroCaja=MC.nTipoRegistroCaja
	--JOIN CAT_FormasPago FP (NOLOCK) ON FP.nFormaPago=1 --<== Efectivo=1
	--WHERE MC.nIDApertura=@nIDApertura
	--     AND nTipoRegistroCaja IN(5) -- <== Pago=5 
	--GROUP BY FP.nFormaPago,FP.cDescripcion

	--Otros Movimientos
	INSERT INTO @Datos
	SELECT FP.nFormaPago,FP.cDescripcion as cFormaPago,SUM(MC.nImporte*MC.nEfecto) as nImporteSistema,
	CASE WHEN FP.nFormaPago=1 THEN 1 ELSE 0 END as bEfectivo
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	--JOIN CAT_TiposRegistroCaja TE (NOLOCK) ON TE.nTipoRegistroCaja=MC.nTipoRegistroCaja
	JOIN CAT_FormasPago FP (NOLOCK) ON FP.nFormaPago=1 --<== Efectivo=1
	WHERE MC.nIDApertura=@nIDApertura AND MC.bActivo=1
	     AND nTipoRegistroCaja NOT IN(5) -- <== Apertura=1 Pago=5 
	GROUP BY FP.nFormaPago,FP.cDescripcion
END

SELECT ISNULL(SUM(nImporteSistema),0) as nDisponible FROM @Datos
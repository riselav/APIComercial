SP_EliminaFuncion 'fn_DesglosarImpuestosArticulo'
GO
CREATE FUNCTION fn_DesglosarImpuestosArticulo (
    @nIdArticulo INT,
    @nPrecioConImpuestos DECIMAL(18, 4)
)
RETURNS TABLE
AS
RETURN
-- SELECT * FROM CAT_ImpuestosArticulos (NOLOCK)
-- SELECT * FROM CAT_Precios (NOLOCK)
-- SELECT * FROM dbo.fn_DesglosarImpuestosArticulo (1,22.2)
   WITH Impuestos AS (
        SELECT 
			idImpuestoIVA = MAX(CASE WHEN i.nTipo = 1 THEN i.nImpuesto END),
			porcentajeIVA = SUM(CASE WHEN i.nTipo = 1 THEN i.nPorcentaje ELSE 0 END),
            iva = SUM(CASE WHEN i.nTipo = 1 THEN i.nPorcentaje / 100.0 ELSE 0 END),
			idImpuestoIEPS = MAX(CASE WHEN i.nTipo = 2 AND ia.bDesglosa = 1 THEN i.nImpuesto END),
            porcentajeIEPS = SUM(CASE 
                WHEN i.nTipo = 2 AND ia.bDesglosa = 1 AND i.bImpuestoImporte = 0 
                THEN i.nPorcentaje / 100.0 
                ELSE 0 
            END),
			tasaIEPS = SUM(CASE 
                WHEN i.nTipo = 2 AND ia.bDesglosa = 1 AND i.bImpuestoImporte = 0 
                THEN i.nPorcentaje / 100.0 
                ELSE 0 
            END),

            montoIEPSFijo = SUM(CASE 
                WHEN i.nTipo = 2 AND ia.bDesglosa = 1 AND i.bImpuestoImporte = 1 
                THEN ia.nImporte 
                ELSE 0 
            END),
			 esIEPSImporte = MAX(CASE 
                WHEN i.nTipo = 2 AND ia.bDesglosa = 1 AND i.bImpuestoImporte = 1 
                THEN 1 ELSE 0 
            END)
        FROM CAT_ImpuestosArticulos ia (NOLOCK)
        JOIN CAT_Impuestos i ON ia.nImpuesto = i.nImpuesto
        WHERE ia.nIDArticulo = @nIdArticulo
    ),
    Base AS (
        SELECT 
            TotalTasa = (1 + ISNULL(iva, 0) + ISNULL(tasaIEPS, 0)),
            idImpuestoIVA,
            porcentajeIVA,
            idImpuestoIEPS,
            porcentajeIEPS,
            iva,
            tasaIEPS,
            montoIEPSFijo,
			esIEPSImporte
        FROM Impuestos
    )
    SELECT 
        PrecioSinImpuestos = CAST(ROUND(@nPrecioConImpuestos / TotalTasa, 2) AS DECIMAL(18,2)),
        IVA = CAST(ROUND((@nPrecioConImpuestos / TotalTasa) * iva, 2) AS DECIMAL(18,2)),
        IEPS = CAST(
            ROUND((@nPrecioConImpuestos / TotalTasa) * tasaIEPS, 2) + montoIEPSFijo
            AS DECIMAL(18,2)
        ),
        idImpuestoIVA,
        porcentajeIVA = CAST(porcentajeIVA AS DECIMAL(5,2)),
        idImpuestoIEPS,
        porcentajeIEPS = CAST(porcentajeIEPS AS DECIMAL(5,2)),
		esIEPSImporte = CAST(esIEPSImporte AS BIT)
    FROM Base;


sp_eliminastore 'VTA_ObtenArticulosVenta_SP_V2'

GO

Create procedure VTA_ObtenArticulosVenta_SP_V2(
	@prmSucursal int,
	@prmListaPrecio int,
	@Criterio varchar(200),
	@Linea int = null
)
AS
BEGIN
    SET NOCOUNT ON;
	-- EXEC VTA_ObtenArticulosVenta_SP_V2 1,1,''

    -- Tabla temporal para guardar resultados
    DECLARE @Resultados TABLE (
        idArticulo INT PRIMARY KEY,
		nPrecio decimal (18,2),
		nExistencia decimal (18,2)
    );

    -- 1. Buscar por Código de Barras
    /*INSERT INTO @Resultados (idArticulo)
    SELECT DISTINCT cb.idArticulo
    FROM CAT_CodigosBarraArticulos cb
    INNER JOIN CAT_Precios p ON p.idArticulo = cb.idArticulo
    WHERE cb.codigoBarra LIKE '%' + @criterio + '%'
      AND p.idSucursal = @idSucursal
      AND p.idListaPrecio = @idListaPrecio;*/

   
    INSERT INTO @Resultados (idArticulo)
    SELECT DISTINCT a.nIDArticulo
    FROM CAT_Articulos a (NOLOCK)
    WHERE a.cClave=@Criterio --a.cClave LIKE '%' + @criterio + '%'
		--AND a.bActivo=1
     
    INSERT INTO @Resultados (idArticulo)
    SELECT DISTINCT a.nIDArticulo
    FROM CAT_Articulos a (NOLOCK)
    WHERE a.cDescripcion LIKE '%' + @criterio + '%' AND a.nLinea = coalesce(@Linea, a.nLinea)
		--AND a.bActivo=1

	UPDATE R SET R.nPrecio=P.nPrecio
	FROM @Resultados R
	JOIN CAT_Precios P (NOLOCK) ON R.idArticulo=P.nIDArticulo
	WHERE p.bActivo=1 AND P.nIdSucursal=@prmSucursal AND P.nIdLista=@prmListaPrecio

	--Buscar precio general si al menos existe un producto que se quedó con precio nulo
	IF EXISTS (SELECT TOP 1 1 FROM @Resultados WHERE nPrecio IS NULL)
	BEGIN
		UPDATE R SET R.nPrecio=P.nPrecio
		FROM @Resultados R
		JOIN CAT_Precios P (NOLOCK) ON R.idArticulo=P.nIDArticulo
		WHERE p.bActivo=1 AND P.nIdSucursal IS NULL AND P.nIdLista=@prmListaPrecio
	END

	UPDATE R SET R.nPrecio=E.nExistencia
	FROM @Resultados R
	JOIN CAT_Sucursales S (NOLOCK) ON S.nSucursal=@prmSucursal
	JOIN IVT_Existencias E (NOLOCK) ON S.nAlmacenInventario=E.nAlmacen
		AND R.idArticulo=E.nIDArticulo
	WHERE e.bActivo=1

    -- Retornar los datos completos
    SELECT distinct
        a.nIDArticulo as Articulo,
        a.cClave as Clave,
        a.cDescripcion as Descripcion,
        r.nPrecio as PrecioUnitarioNeto,
        @prmSucursal as idSucursal,
        @prmListaPrecio as idListaPrecio,
		imp.PrecioSinImpuestos as PrecioUnitarioSinImpuestos,
		imp.IVA as ImpuestoIVAUnitario,
		imp.IdImpuestoIVA,
		imp.PorcentajeIVA as PorcentajeImpuestoIVA,
		imp.IEPS as ImpuestoIEPSUnitario,
		imp.IdImpuestoIEPS,
		imp.PorcentajeIEPS as PorcentajeImpuestoIEPS,
		imp.esIEPSImporte,
		CONVERT (decimal(18,2), 0.01) as CostoUnitario,
		CONVERT (decimal(18,2), 0.0) as ImporteDescuentoUnitario,
		NULL as PorcentajeDescuentoUnitario,
		r.nExistencia as Existencia,
		a.bActivo as Activo,
		li.nLinea,
		li.cDescripcion as DescripcionLinea
    FROM @Resultados r
    INNER JOIN CAT_Articulos a (NOLOCK) ON a.nIDArticulo = r.idArticulo
	LEFT JOIN CAT_Lineas li (NOLOCK) ON a.nLinea = li.nLinea
    left JOIN CAT_Precios p (NOLOCK) ON p.nIDArticulo = a.nIDArticulo
	OUTER APPLY dbo.fn_DesglosarImpuestosArticulo(r.idArticulo, r.nPrecio) imp
    
END
GO



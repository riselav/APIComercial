SP_ELIMINASTORE 'CAT_CON_CatArticulos_SP'
GO
CREATE PROC CAT_CON_CatArticulos_SP(
	@prmMarca varchar(200)='',
	@prmLinea varchar(200)='',
	@prmSublinea varchar(200)='',
	@prmTipoUnidad varchar(150)='',
	@prmUnidad varchar(200)='',
	@prmArticulo varchar(200)=''
)

AS
-- EXEC CAT_CON_CatArticulos_SP  '','','','','',''

SELECT
Art.nIdArticulo,Art.cClave,Art.cDescripcion as cNombreArticulo,Mr.cDescripcion as cNombreMarca,
Ln.cDescripcion as cNombreLinea,Sb.cDescripcion as cNombreSubLinea,Un.cDescripcion as cUnidadInventario,
cc.cDescripcion as cTipoArticulo,cc2.cDescripcion as cTipoUnidad,
CASE WHEN Art.bManejaInventario=1 THEN 'SI' ELSE 'NO' END as cManejaInventario,
CASE WHEN Art.bInsumoFinal=1 THEN 'SI' ELSE 'NO' END as cInsumoFinal,PB.cDescripcion as cProductoBase 
into #Articulos
FROM CAT_Articulos Art (NOLOCK)
LEFT JOIN CAT_Marcas Mr (NOLOCK) ON Mr.nMarca=Art.nMarca
JOIN CAT_Lineas Ln (NOLOCK) ON Ln.nLinea=Art.nLinea
JOIN CAT_SubLineas Sb (NOLOCK) ON Sb.nSubLinea=Art.nSubLinea
	AND Sb.nSubLinea=Art.nSubLinea
JOIN CAT_Unidades Un (NOLOCK) ON Un.nUnidad=Art.nUnidad
JOIN CAT_Catalogos CC (NOLOCK) ON CC.cNombre='CAT_TiposArticulo'
	AND CC.nCodigo=Art.nTipoArticulo
JOIN CAT_Catalogos CC2 (NOLOCK) ON CC2.cNombre='CAT_TiposUnidad'
	AND CC2.nCodigo=Un.nTipoUnidad
LEFT JOIN CAT_ProductosBase PB (NOLOCK) ON PB.nProductoBase=Art.nIdProductoBase
WHERE 1=1

SELECT *
FROM #Articulos
WHERE 1=1
	AND ISNULL(cNombreMarca,'') LIKE CASE
                            WHEN @prmMarca = '' THEN '%'
                            ELSE '%' + @prmMarca + '%'
                          END
	AND ISNULL(cNombreLinea,'') LIKE CASE
                            WHEN @prmLinea = '' THEN '%'
                            ELSE '%' + @prmLinea + '%'
                          END
	AND ISNULL(cNombreSubLinea,'') LIKE CASE
                            WHEN @prmSublinea = '' THEN '%'
                            ELSE '%' + @prmSublinea + '%'
                          END
	AND ISNULL(cTipoUnidad,'') LIKE CASE
                            WHEN @prmTipoUnidad = '' THEN '%'
                            ELSE '%' + @prmTipoUnidad + '%'
                          END
	AND ISNULL(cUnidadInventario,'') LIKE CASE
                            WHEN @prmUnidad = '' THEN '%'
                            ELSE '%' + @prmUnidad + '%'
                          END
	AND ISNULL(cNombreArticulo,'') LIKE CASE
                            WHEN @prmArticulo = '' THEN '%'
                            ELSE '%' + @prmArticulo + '%'
                          END
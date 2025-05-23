--USE [Voalaft_Navola]
--GO
-- [CAT_CON_CatArticulos] 3

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatArticulos'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatArticulos;
END;
GO


CREATE procedure [dbo].[CAT_CON_CatArticulos] (  
@nIDArticulo as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nIDArticulo=0   
Begin  
	set nocount off 
	Select a.nIDArticulo, a.cDescripcion,a.cClave,a.nUnidad, uni.cDescripcion as unidadDescripcion,a.cPresentacion,a.nLinea,lin.cDescripcion as lineaDescripcion
	,a.nSublinea, sublin.cDescripcion as sublineaDescripcion, a.nMarca,mar.cDescripcion as marcaDescripcion,a.cClaveSAT,a.bLote,a.bSerie,a.bPedimento,a.bInsumoFinal
	,a.bProductoBase, pb.cDescripcion as productoBaseDescripcion,a.nIdProductoBase,a.nIdUnidadRelacional,a.nEquivalencia,a.bManejaInventario,a.bActivo,nTipoArticulo,iva.nImpuesto as IdImpuestoIVA ,ieps.nImpuesto as IdImpuestoIEPS, ieps.nImporte as ImporteImpuestoIEPS
	from Cat_Articulos a(nolock) 
	left join CAT_Unidades uni(nolock) on a.nUnidad=uni.nUnidad
	left join CAT_Lineas lin(nolock) on a.nLinea=lin.nLinea
	left join CAT_Sublineas sublin(nolock) on a.nSublinea=sublin.nSublinea
	left join CAT_Marcas mar(nolock) on a.nMarca=mar.nMarca
	left join CAT_ProductosBase pb(nolock) on a.nIdProductoBase=pb.nProductoBase
	left join CAT_ImpuestosArticulos iva(nolock) on a.nIDArticulo=iva.nIDArticulo and iva.nImporte=0 and iva.bDesglosa=0
	left join CAT_ImpuestosArticulos ieps(nolock) on a.nIDArticulo=ieps.nIDArticulo and (ieps.nImporte > 0 or ieps.bDesglosa=1)
End   
Else  
Begin  
	set nocount off 
	Select top 1 a.nIDArticulo, a.cDescripcion,a.cClave,a.nUnidad, uni.cDescripcion as unidadDescripcion,a.cPresentacion,a.nLinea,lin.cDescripcion as lineaDescripcion
	,a.nSublinea, sublin.cDescripcion as sublineaDescripcion, a.nMarca,mar.cDescripcion as marcaDescripcion,a.cClaveSAT,a.bLote,a.bSerie,a.bPedimento,a.bInsumoFinal
	,a.bProductoBase, pb.cDescripcion as productoBaseDescripcion,a.nIdProductoBase,a.nIdUnidadRelacional,a.nEquivalencia,a.bManejaInventario,a.bActivo,nTipoArticulo,iva.nImpuesto as IdImpuestoIVA ,ieps.nImpuesto as IdImpuestoIEPS , ieps.nImporte as ImporteImpuestoIEPS
	from Cat_Articulos a(nolock) 
	left join CAT_Unidades uni(nolock) on a.nUnidad=uni.nUnidad
	left join CAT_Lineas lin(nolock) on a.nLinea=lin.nLinea
	left join CAT_Sublineas sublin(nolock) on a.nSublinea=sublin.nSublinea
	left join CAT_Marcas mar(nolock) on a.nMarca=mar.nMarca
	left join CAT_ProductosBase pb(nolock) on a.nIdProductoBase=pb.nProductoBase
	left join CAT_ImpuestosArticulos iva(nolock) on a.nIDArticulo=iva.nIDArticulo and iva.nImporte=0 and iva.bDesglosa=0
	left join CAT_ImpuestosArticulos ieps(nolock) on a.nIDArticulo=ieps.nIDArticulo and (ieps.nImporte > 0 or ieps.bDesglosa=1)
	where a.nIDArticulo=@nIDArticulo
End  
  
End
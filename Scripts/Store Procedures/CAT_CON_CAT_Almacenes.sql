sp_eliminastore 'CAT_CON_CAT_Almacenes'
GO

CREATE procedure [dbo].[CAT_CON_CAT_Almacenes] (@nFolio int )      
AS      
Begin      
 SELECT A.nAlmacen 
      ,A.cDescripcion  
      ,A.nPlaza  
      ,A.nSucursal  
      ,A.cCodigoPostal  
      ,A.cColonia  
      ,A.cDomicilio
      ,A.cTelefono1  
      ,A.cTelefono2  
      , A.bActivo
	  , P.cDescripcion as cNombrePlaza
	  , S.cDescripcion as cNombreSucursal
	  , Col.cNombreAsentamiento
	  
	  
  FROM CAT_Almacenes As A (Nolock) 
  Inner Join CAT_Plazas as P (Nolock)  on P.nPlaza = A.nPlaza 
  Inner Join CAT_Sucursales as S (Nolock) on S.nSucursal = A.nSucursal 
  Inner Join CAT_Colonias  as Col (Nolock) on Col.cColonia = A.cColonia and Col.cCodigoPostal =A.cCodigoPostal 
 Where nAlmacen = @nFolio or @nFolio=0   
End 
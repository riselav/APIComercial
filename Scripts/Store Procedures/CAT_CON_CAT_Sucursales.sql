sp_eliminaStore 'CAT_CON_CAT_Sucursales'
go

Create procedure CAT_CON_CAT_Sucursales (@nFolio int=0 )    
AS    
-- CAT_CON_CAT_Sucursales
Begin    
 SELECT [nSucursal]
      ,S.cDescripcion
	  ,S.nempresa
	  ,E.cDescripcion as NombreEmpresa
      ,S.nPlaza
	  ,P.cDescripcion as NombrePlaza
	  ,S.nRegion
      ,[cEstado]
      ,[cLocalidad]
      ,[cMunicipio]
      ,S.cCodigoPostal
      ,S.cColonia
	  ,Col.cNombreAsentamiento as NombreColonia
      ,[nZona]
      ,[cDomicilio]
      ,[cTelefono1]
      ,[cTelefono2]
      ,S.bActivo  
 From [dbo].[CAT_Sucursales] S(Nolock)
 Left Join CAT_Empresas E (Nolock) ON S.nEmpresa=E.nEmpresa
 Left Join CAT_Plazas P (Nolock) ON S.nPlaza=P.nPlaza
 Left Join CAT_Colonias Col (Nolock) ON Col.cCodigoPostal=S.cCodigoPostal
	And Col.cColonia=S.cColonia
 Where nSucursal = @nFolio or @nFolio=0

End

sp_eliminaStore 'CAT_CON_CAT_Sucursales'

go

create procedure CAT_CON_CAT_Sucursales (@nFolio int )    
AS    
Begin    
 SELECT [nSucursal]
      ,[cDescripcion]
	  , nempresa
      ,[nPlaza]
	  ,[nRegion]
      ,[cEstado]
      ,[cLocalidad]
      ,[cMunicipio]
      ,[cCodigoPostal]
      ,[cColonia]
      ,[nZona]
      ,[cDomicilio]
      ,[cTelefono1]
      ,[cTelefono2]
      ,[bActivo]  
 From [dbo].[CAT_Sucursales] (Nolock) 
 Where nSucursal = @nFolio or @nFolio=0

End 
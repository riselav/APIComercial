sp_eliminaStore 'CAT_IME_Sucursales'

GO


CREATE procedure CAT_IME_Sucursales (    
@nFolio as int,    
@cDescripcion Varchar(50),    
@nEmpresa int,
@nPlaza as INT,  
@nRegion as INT,  
@cEstado varchar(5),  
@cLocalidad varchar(5),  
@cMunicipio varchar(5),  
@cCodigoPostal varchar(10) ,  
@cColonia varchar(10) ,  
@nZona int ,  
@cDomicilio varchar(300) ,  
@cTelefono1 varchar(20) ,  
@cTelefono2 varchar(20) ,  
@bActivo as bit,    
@cUsuario as Varchar(50),    
@cNombreMaquina as Varchar(50)    
)    
As     
Begin    
    
    
--=================================================================================================================    
-- Si el Folio es cero, indica que es un nuevo registro,    
--=================================================================================================================    
if @nFolio =0     
Begin    
 Declare @nFolioSig as int =  (Select Isnull(max(nSucursal),0)  From CAT_Sucursales) + 1    
    
 Insert into CAT_Sucursales (nSucursal, cDescripcion,nEmpresa, nplaza,nregion,cEstado,cLocalidad,cMunicipio,cCodigoPostal,cColonia,nZona,cDomicilio,cTelefono1,cTelefono2, bactivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)    
 Select @nFolioSig, @cDescripcion,@nEmpresa, @nPlaza,@nRegion,@cEstado,@cLocalidad,@cMunicipio,@cCodigoPostal,@cColonia,@nZona,@cDomicilio,@cTelefono1,@cTelefono2, 1, @cUsuario, @cNombreMaquina, getdate()     
    
 return @nFolioSig    
    
End     
Else    
Begin    
    
--=================================================================================================================    
-- Si el Folio es mayor a cero, se valora el campo bActivo:    
--    Si el valor es 1, entonces indica que es una modificación    
--    Si el valor es 0, entonces se trata de una cancelacion    
--=================================================================================================================    
-- Actualiza el registro indicado    
    
 if @bActivo =1      
 Begin    
      
  Update R Set cDescripcion= @cDescripcion,  
  nplaza = @nPlaza, 
  nEmpresa=@nEmpresa,
  nregion = @nRegion,
  cEstado = @cEstado,  
  cLocalidad = @cLocalidad,  
  cMunicipio = @cMunicipio,  
  cCodigoPostal = @cCodigoPostal,  
  cColonia = @cColonia,  
  nZona = @nZona,  
  cDomicilio = @cDomicilio,  
  cTelefono1 = @cTelefono1,  
  cTelefono2 = @cTelefono2,  
         bactivo= @bActivo,    
      cUsuario_Modifica= @cUsuario,    
      cMaquina_Modifica= @cNombreMaquina,    
      dFecha_Modifica = getdate ()    
  From CAT_Sucursales as R    
  Where nSucursal = @nFolio    
    
  Return @nFolio    
 End     
    
-- Cancela el registro indicado    
     
 Update R Set  bactivo= @bActivo,     
      cUsuario_Cancela= @cUsuario,    
      cMaquina_Cancela= @cNombreMaquina,    
      dFecha_Cancela = getdate ()    
  From CAT_Sucursales as R    
  Where nSucursal = @nFolio    
    
  Return @nFolio    
End    
    
End 
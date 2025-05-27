

sp_eliminastore 'CAT_IME_Proveedores'

GO

--SELECT * FROM cat_impresoras (NOLOCK)
Create procedure CAT_IME_Proveedores (      
@nFolio as int,        
@cDescripcionComercial as Varchar(200),    
@cDescripcionFiscal Varchar(200),      
@cColonia Varchar(50),
@cCodigoPostal Varchar(50),
@cRFC Varchar(20),
@cCURP Varchar(50),
@nTipoPersona tinyint,
@bNacional bit, 
@nDiasCredito tinyint,
@cTelefono Varchar(20),
@cCorreo Varchar(100),
@bActivo bit,
@cUsuario Varchar(50),
@cNombreMaquina Varchar(50)
)
As       
Begin      
   
--=================================================================================================================      
-- Si el Folio es cero, indica que es un nuevo registro,      
--=================================================================================================================      
if @nFolio =0       
Begin      
 Declare @nFolioSig as int =  (Select Isnull(max(nProveedor),0)  From CAT_Proveedores (Nolock)) + 1      
      
 Insert Into CAT_Proveedores (nProveedor, cDescripcionComercial, cDescripcionFiscal, cColonia, cCodigoPostal, 
                              cRFC, cCURP, nTipoPersona, bNacional,  nDiasCredito, cTelefono, cCorreo,bActivo,
							  cUsuario_Registra, cMaquina_Registra, dFecha_Registra) 
 
 Select @nFolioSig,@cDescripcionComercial, @cDescripcionFiscal, @cColonia, @cCodigoPostal,
 @cRFC, @cCURP, @nTipoPersona , @bNacional, @nDiasCredito, @cTelefono, @cCorreo, @bActivo,
 @cUsuario, @cNombreMaquina, getdate()
 
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
        
  Update P Set  cDescripcionComercial=@cDescripcionComercial,
				cDescripcionFiscal=@cDescripcionFiscal,
				cColonia= @cColonia, 
				cCodigoPostal= @cCodigoPostal, 
				cRFC=@cRFC, 
				cCURP= @cCURP, 
				nTipoPersona= @nTipoPersona,
				bNacional= @bNacional,  
				nDiasCredito= @nDiasCredito, 
				cTelefono= @cTelefono, 
				cCorreo= @cCorreo,
				bActivo= @bActivo,
				cUsuario_Modifica= @cUsuario,      
				cMaquina_Modifica= @cNombreMaquina,      
				dFecha_Modifica = getdate ()     
	  
  From CAT_Proveedores as P    
  Where nProveedor = @nFolio      
      
  Return @nFolio      
 End       
      
-- Cancela el registro indicado      
       
 Update R Set  bactivo= @bActivo,       
      cUsuario_Cancela= @cUsuario,      
      cMaquina_Cancela= @cNombreMaquina,      
      dFecha_Cancela = getdate ()      
  From CAT_Proveedores as R      
  Where nProveedor = @nFolio      
      
  Return @nFolio      
End      
      
End 
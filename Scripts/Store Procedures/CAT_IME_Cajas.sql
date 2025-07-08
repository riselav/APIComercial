sp_eliminaStore 'RST_IME_CAT_Cajas'

GO

--SELECT * FROM CAT_Cajas (NOLOCK)
Create procedure RST_IME_CAT_Cajas (        
@nFolio as int,          
@nsucursal as int,      
@cDescripcion Varchar(100),        
@nImpresora as Int,        
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
 Declare @nFolioSig as int =  (Select Isnull(max(nCaja),0)  From CAT_Cajas) + 1        
        
 Insert into CAT_Cajas (nCaja,nSucursal, cDescripcion, nImpresora, bactivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)        
 Select @nFolioSig,@nsucursal, @cDescripcion,@nImpresora, 1, @cUsuario, @cNombreMaquina, getdate()         
        
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
   nsucursal = @nsucursal,      
   nImpresora = @nImpresora,         
         bactivo= @bActivo,        
      cUsuario_Modifica= @cUsuario,        
      cMaquina_Modifica= @cNombreMaquina,        
      dFecha_Modifica = getdate ()        
  From CAT_Cajas as R        
  Where nCaja = @nFolio        
        
  Return @nFolio        
 End         
        
-- Cancela el registro indicado        
         
 Update R Set  bactivo= @bActivo,         
      cUsuario_Cancela= @cUsuario,        
      cMaquina_Cancela= @cNombreMaquina,        
      dFecha_Cancela = getdate ()        
  From CAT_Cajas as R        
  Where nCaja = @nFolio        
        
  Return @nFolio        
End        
        
End 
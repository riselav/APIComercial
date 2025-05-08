sp_eliminastore 'CAT_IME_CAT_TiposMovimientosInventario'

GO

CREATE procedure [dbo].[CAT_IME_CAT_TiposMovimientosInventario] (  
@nTipoMovimiento as int,  
@cDescripcion Varchar(200),
@nEfecto as int,
@bEsCancelacion as bit,
@nContramovimiento int,
@nTipoInvolucrado int, 
@bActivo bit,
@cUsuario as Varchar(50),  
@cNombreMaquina as Varchar(50)  
)  
As   
Begin  


Select * From CAT_TiposMovimientosInventario

--=================================================================================================================  
-- Si el Folio es cero, indica que es un nuevo registro,  
--=================================================================================================================  
if @nTipoMovimiento =0   
Begin  
 Declare @nFolioSig as int =  (Select Isnull(max(nTipoMovimiento),0)  From CAT_TiposMovimientosInventario) + 1  
  
 Insert into CAT_TiposMovimientosInventario (nTipoMovimiento, cDescripcion, nEfecto,  bEsCancelacion, nContramovimiento, nTipoInvolucrado, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)  
 Select @nFolioSig,@cDescripcion, @nEfecto, @bEsCancelacion, @nContramovimiento, @nTipoInvolucrado, 1, @cUsuario, @cNombreMaquina, getdate()   
  
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
    
  Update E Set  cDescripcion= @cDescripcion,  
				nEfecto=@nEfecto,
				bEsCancelacion= @bEsCancelacion, 
				nContramovimiento= @nContramovimiento,
				nTipoInvolucrado= @nTipoInvolucrado,
				bActivo= @bActivo,  
				cUsuario_Modifica= @cUsuario,  
				cMaquina_Modifica= @cNombreMaquina,  
				dFecha_Modifica = getdate ()  

  From CAT_TiposMovimientosInventario as E  
  Where nTipoMovimiento = @nTipoMovimiento 
  
  Return @nTipoMovimiento  
 End   
  
-- Cancela el registro indicado  
   
 Update R Set  bActivo= @bActivo,   
      cUsuario_Cancela= @cUsuario,  
      cMaquina_Cancela= @cNombreMaquina,  
      dFecha_Cancela = getdate ()  
  From CAT_TiposMovimientosInventario as R  
  Where nTipoMovimiento = @nTipoMovimiento
  
  Return @nTipoMovimiento
End  
  
End   
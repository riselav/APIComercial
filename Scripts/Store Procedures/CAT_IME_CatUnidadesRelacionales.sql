sp_eliminastore 'CAT_IME_CatUnidadesRelacionales'
GO

CREATE procedure [dbo].[CAT_IME_CatUnidadesRelacionales] (  
@nUnidadRelacional as int,   
@cDescripcion Varchar(200),
@nTipoUnidad tinyint,
@bEsBase as bit,
@nEquivalencia decimal(18,6),
@bActivo as bit,  
@cUsuario as Varchar(50),  
@cNombreMaquina as Varchar(50)  
)  
As   
Begin  

--=================================================================================================================  
-- Si el Folio es cero, indica que es un nuevo registro,  
--=================================================================================================================  
if @nUnidadRelacional =0   
Begin  
 Declare @nFolioSig as int =  (Select Isnull(max(nUnidadRelacional),0)  From CAT_UnidadesRelacionales) + 1  
  
 Insert into CAT_UnidadesRelacionales (nUnidadRelacional, cDescripcion, nTipoUnidad,bEsBase,nEquivalencia, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)  
 Select @nFolioSig,@cDescripcion, @nTipoUnidad,@bEsBase,@nEquivalencia, 1, @cUsuario, @cNombreMaquina, getdate()   
  
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
    
  Update E Set cDescripcion= @cDescripcion,  	 
		 nTipoUnidad=@nTipoUnidad,
		 bEsBase=@bEsBase,
		 nEquivalencia=@nEquivalencia,
		 bActivo= @bActivo,  
         cUsuario_Modifica= @cUsuario,  
         cMaquina_Modifica= @cNombreMaquina,  
         dFecha_Modifica = getdate ()  
  From CAT_UnidadesRelacionales as E  
  Where nUnidadRelacional = @nUnidadRelacional
  
  Return @nUnidadRelacional  
 End   
  
-- Cancela el registro indicado  
   
 Update R Set  bActivo= @bActivo,   
      cUsuario_Cancela= @cUsuario,  
      cMaquina_Cancela= @cNombreMaquina,  
      dFecha_Cancela = getdate ()  
  From CAT_UnidadesRelacionales as R  
  Where nUnidadRelacional = @nUnidadRelacional
  
  Return @nUnidadRelacional
End  
  
End   
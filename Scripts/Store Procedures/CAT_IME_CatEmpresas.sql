sp_eliminastore 'CAT_IME_CatEmpresas'
GO

CREATE procedure [dbo].[CAT_IME_CatEmpresas] (  
@nEmpresa as int,  
@nGrupoEmpresarial as int,  
@cDescripcion Varchar(200),
@bActivo as bit,  
@cUsuario as Varchar(50),  
@cNombreMaquina as Varchar(50)  
)  
As   
Begin  

--=================================================================================================================  
-- Si el Folio es cero, indica que es un nuevo registro,  
--=================================================================================================================  
if @nEmpresa =0   
Begin  
 Declare @nFolioSig as int =  (Select Isnull(max(nEmpresa),0)  From CAT_Empresas) + 1  
  
 Insert into CAT_Empresas (nEmpresa,nGrupoEmpresarial, cDescripcion, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)  
 Select @nFolioSig,@nGrupoEmpresarial,@cDescripcion, 1, @cUsuario, @cNombreMaquina, getdate()   
  
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
    
  Update E Set nGrupoEmpresarial = @nGrupoEmpresarial,
		 cDescripcion= @cDescripcion,  
		 bActivo= @bActivo,  
         cUsuario_Modifica= @cUsuario,  
         cMaquina_Modifica= @cNombreMaquina,  
         dFecha_Modifica = getdate ()  
  From CAT_Empresas as E  
  Where nEmpresa = @nEmpresa
  
  Return @nEmpresa  
 End   
  
-- Cancela el registro indicado  
   
 Update R Set  bActivo= @bActivo,   
      cUsuario_Cancela= @cUsuario,  
      cMaquina_Cancela= @cNombreMaquina,  
      dFecha_Cancela = getdate ()  
  From CAT_Empresas as R  
  Where nEmpresa = @nEmpresa
  
  Return @nEmpresa
End  
  
End   
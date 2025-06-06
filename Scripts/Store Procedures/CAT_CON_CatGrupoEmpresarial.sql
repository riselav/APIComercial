sp_eliminastore 'CAT_CON_CatGrupoEmpresarial'
GO

CREATE procedure [dbo].[CAT_CON_CatGrupoEmpresarial] (  
@nGrupoEmpresarial as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nGrupoEmpresarial=0   
Begin  
	set nocount off 
	Select nGrupoEmpresarial, cDescripcion, bActivo from Cat_GrupoEmpresarial (nolock)  
End   
Else  
Begin  
	set nocount off 
	Select nGrupoEmpresarial, cDescripcion, bActivo from Cat_GrupoEmpresarial (nolock) where nGrupoEmpresarial=@nGrupoEmpresarial
End  
  
End
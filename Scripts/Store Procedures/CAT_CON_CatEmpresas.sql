sp_eliminastore 'CAT_CON_CatEmpresas'
GO


CREATE procedure [dbo].[CAT_CON_CatEmpresas] (  
@nEmpresa as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nEmpresa=0   
Begin  
	set nocount off 
	Select nEmpresa, sl.cDescripcion,sl.nGrupoEmpresarial, l.cDescripcion as cDescripcionGrupoEmpresarial, sl.bActivo from Cat_Empresas sl (nolock) inner join CAT_GrupoEmpresarial l (nolock) on sl.nGrupoEmpresarial=l.nGrupoEmpresarial  
End   
Else  
Begin  
	set nocount off 
	Select nEmpresa, sl.cDescripcion,sl.nGrupoEmpresarial, l.cDescripcion as cDescripcionGrupoEmpresarial, sl.bActivo from Cat_Empresas sl (nolock) inner join CAT_GrupoEmpresarial l (nolock) on sl.nGrupoEmpresarial=l.nGrupoEmpresarial 
	where nEmpresa=@nEmpresa
End  
  
End
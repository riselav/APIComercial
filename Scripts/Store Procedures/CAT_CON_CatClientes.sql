
SP_EliminaStore 'CAT_CON_CatCatalogos_por_nombre'


CREATE procedure [dbo].[CAT_CON_CatCatalogos_por_nombre] (  
@cNombre as varchar(50)
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @cNombre = ''   
Begin  
	set nocount off 
	Select nCatalogo, cNombre,nCodigo, cDescripcion, bActivo from CAT_Catalogos (nolock)  
End   
Else  
Begin  
	set nocount off 
	Select nCatalogo, cNombre,nCodigo, cDescripcion, bActivo from CAT_Catalogos (nolock) where cNombre=@cNombre
End  
  
End
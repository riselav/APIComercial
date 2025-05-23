sp_eliminastore 'CAT_CON_CatUnidadesRelacionales'
GO



CREATE procedure [dbo].[CAT_CON_CatUnidadesRelacionales] (  
@nUnidadRelacional as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nUnidadRelacional=0   
Begin  
	set nocount off 
	Select pb.nUnidadRelacional, pb.cDescripcion,pb.nTipoUnidad,cat.cDescripcion as DescripcionTipoUnidad,bEsBase,nEquivalencia,pb.bActivo from Cat_UnidadesRelacionales pb (nolock) left join CAT_Catalogos cat (nolock) 
	on (pb.nTipoUnidad=cat.nCodigo and cat.cNombre='CAT_TIPO_UNIDAD')
End   
Else  
Begin  
	set nocount off 
	Select pb.nUnidadRelacional, pb.cDescripcion,pb.nTipoUnidad,cat.cDescripcion as DescripcionTipoUnidad,bEsBase,nEquivalencia,pb.bActivo from Cat_UnidadesRelacionales pb (nolock) left join CAT_Catalogos cat (nolock) 
	on (pb.nTipoUnidad=cat.nCodigo and cat.cNombre='CAT_TIPO_UNIDAD')
	where nUnidadRelacional=@nUnidadRelacional
End  
  
End
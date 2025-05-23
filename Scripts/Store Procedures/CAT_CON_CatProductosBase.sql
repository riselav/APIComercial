--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_CON_CatProductosBase'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_CON_CatProductosBase;
END;
GO


CREATE procedure [dbo].[CAT_CON_CatProductosBase] (  
@nProductoBase as int
)  
As   
Begin  

set nocount on
--=================================================================================================================  
-- Si el Folio es cero, Consulto toda la informacion
--=================================================================================================================  
if @nProductoBase=0   
Begin  
	set nocount off 
	Select pb.nProductoBase, pb.cDescripcion,pb.nTipoUnidad,cat.cDescripcion as DescripcionTipoUnidad,pb.bActivo from Cat_ProductosBase pb (nolock) left join CAT_Catalogos cat (nolock) 
	on (pb.nTipoUnidad=cat.nCodigo and cat.cNombre='CAT_TIPO_UNIDAD')
End   
Else  
Begin  
	set nocount off 
	Select pb.nProductoBase, pb.cDescripcion,pb.nTipoUnidad,cat.cDescripcion as DescripcionTipoUnidad,pb.bActivo from Cat_ProductosBase pb (nolock) left join CAT_Catalogos cat (nolock) 
	on (pb.nTipoUnidad=cat.nCodigo and cat.cNombre='CAT_TIPO_UNIDAD')
	where nProductoBase=@nProductoBase
End  
  
End
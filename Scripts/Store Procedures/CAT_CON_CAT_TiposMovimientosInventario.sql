sp_eliminastore 'CAT_CON_CAT_TiposMovimientosInventario'

GO
-- Exec CAT_CON_CAT_TiposMovimientosInventario 1
CREATE procedure [dbo].[CAT_CON_CAT_TiposMovimientosInventario] (  
@nTipoMovimiento as int
)  
As   
Begin  

set nocount on


Select TM.nTipoMovimiento, TM.cDescripcion as cDescripcion, TM.nEfecto, TE.cDescripcion as cEfecto,  TM.nTipoInvolucrado, TI.cDescripcion as cTipoInvolucrado,
TM.bEsCancelacion, RSN.cDescripcion as cEsCancelacion, TM.nContramovimiento, C.cDescripcion as cContramovimiento, TM.nTipoInvolucrado, TI.cDescripcion as cTipoInvolucrado, 
TM.bActivo
From CAT_TiposMovimientosInventario as TM (Nolock)
Inner Join CAT_Catalogos as TE (Nolock) on TE.cNombre ='CAT_EFECTOSINVENTARIOS' and TE.nCodigo = TM.nEfecto 
Inner Join CAT_Catalogos as TI (NOLOCK) on TI.cNombre ='CAT_TIPOSINVOLUCRADOS' and TI.nCodigo = TM.nTipoInvolucrado 
Inner Join CAT_Catalogos as RSN (NOLOCK) on RSN.cNombre ='CAT_RESPUESTASINO' and RSN.nCodigo = TM.bEsCancelacion  
Inner Join CAT_TiposMovimientosInventario as C (Nolock) on C.nTipoMovimiento = TM.nContramovimiento 
Where TM.nTipoMovimiento = @nTipoMovimiento or @nTipoMovimiento=0
  
End


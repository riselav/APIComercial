SP_ELIMINASTORE 'CAT_CON_CajasSucursal_SP'
GO
CREATE procedure [dbo].[CAT_CON_CajasSucursal_SP] (
	@nCaja int=0,
	@nSucursal int=0
)  
AS   
--CAT_CON_CajasSucursal_SP 0,1

SELECT CC.nCaja,CC.cDescripcion,CC.nSucursal,S.cDescripcion as cNombreSucursal,CC.nImpresora,CC.bActivo,
CC.cUsuario_Registra,CC.cMaquina_Registra
INTO #Cajas
FROM CAT_Cajas CC(NOLOCK)
JOIN CAT_Sucursales S (NOLOCK) ON S.nSucursal=CC.nSucursal
WHERE CC.nSucursal= CASE WHEN @nSucursal=0 THEN CC.nSucursal ELSE @nSucursal END

IF @nCaja>0 DELETE #Cajas WHERE nCaja<> @nCaja -- Como @nCaja es la llave, este parametro tendría prioridad y quitaría el resto que no sea ese

SELECT * FROM #Cajas

SP_ELIMINASTORE 'SAT_CON_RegimenFiscal'
GO
CREATE procedure [dbo].[SAT_CON_RegimenFiscal] (
	@nRegimenFiscal int=0
)  
AS   
--SAT_CON_RegimenFiscal 601
	
SELECT nRegimenFiscal,cDescripcion,bFisica,bMoral
FROM SAT_CatRegimenFiscal (NOLOCK)
WHERE nRegimenFiscal= CASE WHEN @nRegimenFiscal=0 THEN nRegimenFiscal ELSE @nRegimenFiscal END
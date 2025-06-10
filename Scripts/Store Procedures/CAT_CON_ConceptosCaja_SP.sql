SP_ELIMINASTORE 'CAT_CON_ConceptosCaja_SP'
GO
CREATE procedure [dbo].[CAT_CON_ConceptosCaja_SP] (
	@nConceptoCaja int=0,
	@nEfecto smallint=0
)  
AS   
--CAT_CON_ConceptosCaja_SP 2
--CAT_CON_ConceptosCaja_SP 0,0
--CAT_CON_ConceptosCaja_SP 0,-1
	
SELECT nConceptoCaja,cDescripcion,nEfecto,bActivo,cUsuario_Registra,cMaquina_Registra
FROM CAT_ConceptosCaja (NOLOCK)
WHERE nConceptoCaja= CASE WHEN @nConceptoCaja=0 THEN nConceptoCaja ELSE @nConceptoCaja END
	AND nEfecto=CASE WHEN @nEfecto=0 THEN nEfecto ELSE @nEfecto END
	AND bActivo=1
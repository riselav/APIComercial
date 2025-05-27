SP_ELIMINASTORE 'CAT_CON_Turnos_SP'
GO
CREATE procedure [dbo].[CAT_CON_Turnos_SP] (
	@nTurno int=0
)  
AS   
--CAT_CON_Turnos_SP 3
	
SELECT nTurno,cDescripcion,bActivo,cUsuario_Registra,cMaquina_Registra
FROM CAT_Turnos (NOLOCK)
WHERE nTurno= CASE WHEN @nTurno=0 THEN nTurno ELSE @nTurno END
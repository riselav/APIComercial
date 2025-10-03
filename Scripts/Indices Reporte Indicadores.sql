--sqlcmd -S .\SQLEXPRESS01 -d VoalaftSalsasProd -Q  "CREATE NONCLUSTERED INDEX IDX_MovtosCaja_Corte ON [dbo].[CAJ_MovimientosCaja] ([bActivo],[nIDCorteCaja]) INCLUDE ([nTipoRegistroCaja],[nSucursal],[nIDApertura],[nConceptoCaja],[nEmpleadoInvolucrado],[nImporte],[nEfecto],[dFecha],[nFecha],[cObservaciones],[cUsuario_Registra],[cMaquina_Registra],[dFecha_Registra],[cUsuario_Modifica],[cMaquina_Modifica],[dFecha_Modifica],[cUsuario_Cancela],[cMaquina_Cancela],[dFecha_Cancela],[nConsecutivo],[cUsuarioAutoriza_Registro],[cUsuarioAutoriza_Modificacion],[cUsuarioAutoriza_Cancelacion],[nImporte_Respaldo],[bRegistroEspecial],[bCancelado])" -t 0

CREATE NONCLUSTERED INDEX IDX_MovtosCaja_Corte ON [dbo].[CAJ_MovimientosCaja] ([bActivo],[nIDCorteCaja]) 
INCLUDE ([nTipoRegistroCaja],[nSucursal],[nIDApertura],[nConceptoCaja],[nEmpleadoInvolucrado],[nImporte],[nEfecto],[dFecha],[nFecha],[cObservaciones],[cUsuario_Registra],[cMaquina_Registra],[dFecha_Registra],[cUsuario_Modifica],[cMaquina_Modifica],[dFecha_Modifica],[cUsuario_Cancela],[cMaquina_Cancela],[dFecha_Cancela],[nConsecutivo],[cUsuarioAutoriza_Registro],[cUsuarioAutoriza_Modificacion],[cUsuarioAutoriza_Cancelacion],[nImporte_Respaldo],[bRegistroEspecial],[bCancelado])


CREATE NONCLUSTERED INDEX IDX_Reg_OrdenesEncabezado_Est ON [dbo].[REG_OrdenesEncabezado] ([nEstatus]) 
INCLUDE ([nMesa],[nTipoServicio],[nDescuento],[nTotal],[nEmpleadoAbreMesa],[nCliente],[nDescuento_Respaldo])


CREATE NONCLUSTERED INDEX IDX_MovtosCaja_Apert ON [dbo].[CAJ_MovimientosCaja] ([nEfecto],[bActivo]) INCLUDE ([nIDApertura],[nImporte])

CREATE NONCLUSTERED INDEX IDX_MovtosCaja_TipoRegEf ON [dbo].[CAJ_MovimientosCaja] ([nTipoRegistroCaja],[nEfecto],[bActivo]) INCLUDE ([nIDApertura],[nImporte],[bRegistroEspecial])
CREATE NONCLUSTERED INDEX IDX_MovtosCaja_TipoRegEfCC ON [dbo].[CAJ_MovimientosCaja] ([nTipoRegistroCaja],[nEfecto],[bActivo]) INCLUDE ([nIDApertura],[nConceptoCaja],[nImporte],[bRegistroEspecial])

CREATE NONCLUSTERED INDEX IDX_Reg_OrdenesEncabezado_Apert ON [dbo].[REG_OrdenesEncabezado] ([nEstatus]) INCLUDE ([nIDApertura])

/*
SELECT 
    transaction_id,
    name,
    transaction_state,
    transaction_begin_time 
FROM sys.dm_tran_active_transactions;

select * from  sys.dm_exec_query_resource_semaphores

SELECT 
    t1.session_id AS 'Sesion Bloqueada',
    t2.session_id AS 'Sesion Bloqueadora',
    DB_NAME(t2.database_id) AS 'Base de Datos',
    t2.command AS 'Comando Bloqueador',
    t2.wait_type AS 'Tipo de Espera',
    t2.wait_resource AS 'Recurso'
FROM sys.dm_exec_requests t1
JOIN sys.dm_exec_requests t2 ON t1.blocking_session_id = t2.session_id
WHERE t1.blocking_session_id <> 0;


EXEC sp_who2 'active'
*/
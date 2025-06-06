/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 29-oct.-2023 10:53:25 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */


/* Create Tables */
IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[CAJ_RegistrosAperturaCaja]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
BEGIN
CREATE TABLE [CAJ_RegistrosAperturaCaja]
(
	[nIDApertura] bigint NOT NULL,    -- id
	[nIDSucursal] int NOT NULL,    -- id de sucursal a la que pertenece el registro
	[nIDCaja] int NOT NULL,    -- id de caja de la sucursal en la que se hace la apertura
	[nIDTurno] int NOT NULL,    -- id de catálogo de turno en que se hace el registro de la apertura
	[dFecha] datetime NOT NULL,    -- fecha de la apertura de caja
	[nFecha] int NULL,    -- representacion numérica de la fecha de la apertura
	[nDotacionInicial] decimal(18,4) NOT NULL,
	[nIDEmpleado] int NOT NULL,    -- id de catálogo de empleado al que pertenece el registro de la apertura
	[nIDUsuarioAutoriza] int NULL,    -- id de usuario que autoriza el registro de la apetura, que puede ser el mismo empleado si tiene asignado una operacion restringida o si es otra persona con permiso a dicho registro
	[nEstatus] tinyint NOT NULL,    -- estatus de la apertura  1=Abierta 0=Cerrada
	[bActivo] bit NOT NULL,    -- campo que indica si está activo o no el registro
	[cUsuario_Registra] varchar(50) NOT NULL,    -- login del usuario que realiza el registro
	[cMaquina_Registra] varchar(50) NOT NULL,    -- nombre del equipo desde donde se realiza el registro
	[dFecha_Registra] datetime NOT NULL,    -- fecha en que se hace el registro
	[cUsuario_Modifica] varchar(50) NULL,    -- login del usuario que realiza la ultima modificación del registro
	[cMaquina_Modifica] varchar(50) NULL,    -- nombre del equipo desde donde se realiza la ultima modificación del registro
	[dFecha_Modifica] datetime NULL,    -- fecha en que se hace la ultima modificación del registro
	[cUsuario_Cancela] varchar(50) NULL,    -- login del usuario que realiza la ultima inactivación del registro
	[cMaquina_Cancela] varchar(50) NULL,    -- nombre del equipo desde donde se realiza la ultima inactivación del registro
	[dFecha_Cancela] datetime NULL    -- fecha en que se hace la ultima inactivación del registro
)

	ALTER TABLE [CAJ_RegistrosAperturaCaja] ADD CONSTRAINT [PK_CAJ_RegistrosAperturaCaja]
	PRIMARY KEY CLUSTERED ([nIDApertura] ASC)
END	
GO


/* Create Foreign Key Constraints */
IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAJ_RegistrosAperturaCaja_CAT_Cajas' AND xtype = 'F')
	ALTER TABLE [CAJ_RegistrosAperturaCaja] ADD CONSTRAINT [FK_CAJ_RegistrosAperturaCaja_CAT_Cajas]
	FOREIGN KEY ([nIDCaja]) REFERENCES [dbo].[CAT_Cajas] ([nCaja]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAJ_RegistrosAperturaCaja_CAT_Empleados' AND xtype = 'F')
	ALTER TABLE [CAJ_RegistrosAperturaCaja] ADD CONSTRAINT [FK_CAJ_RegistrosAperturaCaja_CAT_Empleados]
	FOREIGN KEY ([nIDEmpleado]) REFERENCES [CAT_Empleados] ([nEmpleado]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAJ_RegistrosAperturaCaja_CAT_Sucursales' AND xtype = 'F')
	ALTER TABLE [CAJ_RegistrosAperturaCaja] ADD CONSTRAINT [FK_CAJ_RegistrosAperturaCaja_CAT_Sucursales]
	FOREIGN KEY ([nIDSucursal]) REFERENCES [dbo].[CAT_Sucursales] ([nSucursal]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAJ_RegistrosAperturaCaja_CAT_Turnos' AND xtype = 'F')
	ALTER TABLE [CAJ_RegistrosAperturaCaja] ADD CONSTRAINT [FK_CAJ_RegistrosAperturaCaja_CAT_Turnos]
	FOREIGN KEY ([nIDTurno]) REFERENCES [CAT_Turnos] ([nTurno]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAJ_RegistrosAperturaCaja_CAT_Usuarios' AND xtype = 'F')
	ALTER TABLE [CAJ_RegistrosAperturaCaja] ADD CONSTRAINT [FK_CAJ_RegistrosAperturaCaja_CAT_Usuarios]
	FOREIGN KEY ([nIDUsuarioAutoriza]) REFERENCES [CAT_Usuarios] ([nFolio]) ON DELETE No Action ON UPDATE No Action
GO
/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 29-oct.-2023 10:53:25 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Create Tables */
IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[CAT_Turnos]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
BEGIN
CREATE TABLE [CAT_Turnos]
(
	[nTurno] int NOT NULL,    -- id
	[cDescripcion] varchar(200) NOT NULL,    -- nombre del registro
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
	ALTER TABLE [CAT_Turnos] ADD CONSTRAINT [PK_CAT_Turnos]
	PRIMARY KEY CLUSTERED ([nTurno] ASC)
END
GO
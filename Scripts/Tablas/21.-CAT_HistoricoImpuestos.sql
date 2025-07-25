/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 05-feb.-2025 10:36:05 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Create Tables */
IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[CAT_HistoricoImpuestos]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
BEGIN
CREATE TABLE [CAT_HistoricoImpuestos]
(
	[nID] int NOT NULL,    -- llave
	[nImpuesto] int NOT NULL,    -- id del impuesto relacionado
	[nPorcentaje] decimal(18,2) NOT NULL,    -- porcentaje de impuesto a usar en el rango de fecha especificado del 
	[dFechaInicio] datetime NOT NULL,    -- fecha de inicio del valor del porcentaje para el id de impuesto indicado
	[dFechaFin] datetime NULL,    -- fecha en que termina el uso del porcentaje indicado para el id del impuesto relacionado
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

	/* Create Primary Keys, Indexes, Uniques, Checks */

	ALTER TABLE [CAT_HistoricoImpuestos] ADD CONSTRAINT [PK_CAT_HistoricoImpuestos]
	PRIMARY KEY CLUSTERED ([nID] ASC)
	
	ALTER TABLE [CAT_HistoricoImpuestos] 
	ADD CONSTRAINT [UQ_ValorImpuesto] UNIQUE NONCLUSTERED ([nImpuesto] ASC,[nPorcentaje] ASC)
END
GO

/* Create Foreign Key Constraints */
IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_HistoricoImpuestos_CAT_Impuestos' AND xtype = 'F')
	ALTER TABLE [CAT_HistoricoImpuestos] ADD CONSTRAINT [FK_CAT_HistoricoImpuestos_CAT_Impuestos]
	FOREIGN KEY ([nImpuesto]) REFERENCES [CAT_Impuestos] ([nImpuesto]) ON DELETE No Action ON UPDATE No Action
GO
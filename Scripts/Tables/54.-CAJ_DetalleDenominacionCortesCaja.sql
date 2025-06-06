/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 05-mar.-2024 11:01:12 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */


/* Create Tables */

IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[CAJ_DetalleDenominacionCortesCaja]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
BEGIN
CREATE TABLE [CAJ_DetalleDenominacionCortesCaja]
(
	[nIDCorteCaja] bigint NOT NULL,    -- detalle del corte de caja al que pertenece el registro
	[nRenglon] smallint NOT NULL,    -- renglon del detalle de corte de caja al que corresponde el registro, la idea seria que solo aplique para cuando se indica parte en efectivo
	[nDenominacion] int NOT NULL,    -- id de catalogo de denominación al que pertenece el registro
	[nValor] decimal(18,4) NOT NULL,    -- valor unitario de la denominacion incluida en el registro
	[nCantidad] int NOT NULL,    -- cantidad registrada de la denominación indicada
	[nImporte] decimal(18,4) NOT NULL,    -- importe resultado de multiplicar la cantidad de la denominación por su valor unitario
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

	ALTER TABLE [CAJ_DetalleDenominacionCortesCaja] ADD CONSTRAINT [PK_CAJ_DetalleDenominacionCortesCaja]
	PRIMARY KEY CLUSTERED ([nIDCorteCaja] ASC,[nRenglon] ASC,[nDenominacion] ASC)
END
GO

/* Create Foreign Key Constraints */
IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAJ_DetalleDenominacionCortesCaja_CAJ_DetalleCorteCaja' AND xtype = 'F')
	ALTER TABLE [CAJ_DetalleDenominacionCortesCaja] ADD CONSTRAINT [FK_CAJ_DetalleDenominacionCortesCaja_CAJ_DetalleCorteCaja]
	FOREIGN KEY ([nIDCorteCaja],[nRenglon]) REFERENCES [CAJ_DetalleCorteCaja] ([nIDCorteCaja],[nRenglon]) ON DELETE No Action ON UPDATE No Action
GO
/*
select d.* 
from CAJ_DetalleDenominacionCortesCaja  d
left join CAJ_DetalleCorteCaja dc on d.nIDCorteCaja=dc.nIDCorteCaja
	and d.nRenglon=dc.nRenglon
where dc.nIDCorteCaja is null
*/

/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 19-feb.-2025 11:43:58 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Create Tables */
IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[CAT_Impresoras]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN
	CREATE TABLE [dbo].[CAT_Impresoras]
	(
		[nImpresora] int NOT NULL,    -- id
		[nSucursal] int NOT NULL,    -- id de sucursal a la que pertenece el registro
		[cDescripcion] varchar(100) NOT NULL,    -- nombre de la impresora
		[cRuta] varchar(100) NOT NULL,    -- ruta compartida de la impresora
		[nTipoEmulacion] smallint NOT NULL,    -- tipo de ejecuci�n de la impresi�n  valores:  1= 2=
		[bActivo] bit NOT NULL,    -- indicador de si el registro est� activo
		[cUsuario_Registra] varchar(50) NULL,
		[cMaquina_Registra] varchar(50) NULL,
		[dFecha_Registra] smalldatetime NULL,
		[cUsuario_Modifica] varchar(50) NULL,
		[cMaquina_Modifica] varchar(50) NULL,
		[dFecha_Modifica] varchar(50) NULL,
		[cUsuario_Cancela] varchar(50) NULL,
		[cMaquina_Cancela] varchar(50) NULL,
		[dFecha_Cancela] smalldatetime NULL
	)


	/* Create Primary Keys, Indexes, Uniques, Checks */
	ALTER TABLE [dbo].[CAT_Impresoras] ADD CONSTRAINT [PK_CAT_Impresoras]
		PRIMARY KEY CLUSTERED ([nImpresora] ASC)
END
GO

/* Create Foreign Key Constraints */
IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Impresoras_CAT_Sucursales' AND xtype = 'F')
	ALTER TABLE [dbo].[CAT_Impresoras] ADD CONSTRAINT [FK_CAT_Impresoras_CAT_Sucursales]
	FOREIGN KEY ([nSucursal]) REFERENCES [dbo].[CAT_Sucursales] ([nSucursal]) ON DELETE No Action ON UPDATE No Action
GO


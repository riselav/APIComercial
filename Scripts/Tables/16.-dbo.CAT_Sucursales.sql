/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 05-feb.-2025 10:36:06 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Create Tables */
IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[CAT_Sucursales]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
BEGIN
CREATE TABLE [dbo].[CAT_Sucursales]
(
	[nSucursal] int NOT NULL,    -- id, llave
	[cDescripcion] varchar(200) NOT NULL,    -- nombre de la sucursal
	[nPlaza] int NOT NULL,    -- id de catalog de plaza asignado a la sucursal
	[nRegion] int NULL,    -- id de catalog de regiones asignado a la sucursal
	[cEstado] varchar(5) NULL,    -- clave de estado de c�digo postal
	[cLocalidad] varchar(5) NULL,    -- clave de localidad de c�digo postal
	[cMunicipio] varchar(5) NULL,    -- clave de municipio de c�digo postal
	[cCodigoPostal] varchar(50) NULL,    -- clave de c�digo postal
	[cColonia] varchar(50) NULL,    -- clave de catalogo de colonias
	[nZona] int NULL,    -- id de catalog de zona asignado a la sucursal
	[cDomicilio] varchar(300) NOT NULL,    -- direccion de la sucursal
	[cTelefono1] varchar(20) NOT NULL,    -- n�mero de tel�fono principal de la sucursal
	[cTelefono2] varchar(20) NOT NULL,    -- n�mero de tel�fono secundario de la sucursal
	[bActivo] bit NOT NULL,
	[cUsuario_Registra] varchar(50) NULL,
	[cMaquina_Registra] varchar(50) NULL,
	[dFecha_Registra] smalldatetime NULL,
	[cUsuario_Modifica] varchar(50) NULL,
	[cMaquina_Modifica] varchar(50) NULL,
	[dFecha_Modifica] varchar(50) NULL,
	[cUsuario_Cancela] varchar(50) NULL,
	[cMaquina_Cancela] varchar(50) NULL,
	[dFecha_Cancela] smalldatetime NULL,
	[nEmpresa] int NOT NULL    -- id de cat�logo de empresa a la que pertene la sucursal
)


	/* Create Primary Keys, Indexes, Uniques, Checks */

	ALTER TABLE [dbo].[CAT_Sucursales] ADD CONSTRAINT [PK_CAT_Sucursales]
	PRIMARY KEY CLUSTERED ([nSucursal] ASC)
END
GO

/* Create Foreign Key Constraints */
IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Sucursales_CAT_Colonias' AND xtype = 'F')
	ALTER TABLE [dbo].[CAT_Sucursales] ADD CONSTRAINT [FK_CAT_Sucursales_CAT_Colonias]
	FOREIGN KEY ([cColonia],[cCodigoPostal]) REFERENCES [dbo].[CAT_Colonias] ([cColonia],[cCodigoPostal]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Sucursales_CAT_Empresas' AND xtype = 'F')
	ALTER TABLE [dbo].[CAT_Sucursales] ADD CONSTRAINT [FK_CAT_Sucursales_CAT_Empresas]
	FOREIGN KEY ([nEmpresa]) REFERENCES [dbo].[CAT_Empresas] ([nEmpresa]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Sucursales_CAT_Plazas' AND xtype = 'F')
	ALTER TABLE [dbo].[CAT_Sucursales] ADD CONSTRAINT [FK_CAT_Sucursales_CAT_Plazas]
	FOREIGN KEY ([nPlaza]) REFERENCES [dbo].[CAT_Plazas] ([nPlaza]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Sucursales_CAT_Regiones' AND xtype = 'F')
	ALTER TABLE [dbo].[CAT_Sucursales] ADD CONSTRAINT [FK_CAT_Sucursales_CAT_Regiones]
	FOREIGN KEY ([nRegion]) REFERENCES [dbo].[CAT_Regiones] ([nRegion]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Sucursales_CAT_Zonas' AND xtype = 'F')
	ALTER TABLE [dbo].[CAT_Sucursales] ADD CONSTRAINT [FK_CAT_Sucursales_CAT_Zonas]
	FOREIGN KEY ([nZona]) REFERENCES [dbo].[CAT_Zonas] ([nZona]) ON DELETE No Action ON UPDATE No Action
GO

IF dbo.fn_ExisteCampo('CAT_Sucursales','nAlmacenInventario')=0
	ALTER TABLE CAT_Sucursales ADD nAlmacenInventario int
GO


IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Sucursales_CAT_Almacenes' AND xtype = 'F')
	ALTER TABLE [dbo].[CAT_Sucursales] ADD CONSTRAINT [FK_CAT_Sucursales_CAT_Almacenes]
	FOREIGN KEY ([nAlmacenInventario]) REFERENCES [dbo].[CAT_Almacenes] ([nAlmacen]) ON DELETE No Action ON UPDATE No Action
GO
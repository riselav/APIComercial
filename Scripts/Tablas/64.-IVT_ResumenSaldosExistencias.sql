/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 20-mar.-2025 12:23:43 a. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Create Tables */
IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[IVT_ResumenSaldosExistencias]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
BEGIN
CREATE TABLE [IVT_ResumenSaldosExistencias]
(
	[nAlmacen] int NOT NULL,    -- id de cat�logo de almac�n al que pertenece el registro
	[nIDArticulo] int NOT NULL,    -- id de cat�logo de art�culos al que pertenece el registro
	[nAnio] smallint NOT NULL,    -- a�o del que se quiere llevar el resumen de saldo
	[nCantidadSaldoInicial] decimal(18,4) NOT NULL,    -- valor de la cantidad de existencia con la que inicia el art�culo en el almac�n en el a�o indicado
	[nImporteSaldoInicial] decimal(18,4) NOT NULL,    -- valor de inventario con la que inicia el art�culo en el almac�n en el a�o indicado
	[nEntradas01] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 1 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte01] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 1 del art�culo en el almac�n en el a�o indicado
	[nEntradas02] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 2 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte02] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 2 del art�culo en el almac�n en el a�o indicado
	[nEntradas03] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 3 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte03] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 3 del art�culo en el almac�n en el a�o indicado
	[nEntradas04] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 4 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte04] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 4 del art�culo en el almac�n en el a�o indicado
	[nEntradas05] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 5 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte05] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 5 del art�culo en el almac�n en el a�o indicado
	[nEntradas06] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 6 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte06] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 6 del art�culo en el almac�n en el a�o indicado
	[nEntradas07] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 7 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte07] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 7 del art�culo en el almac�n en el a�o indicado
	[nEntradas08] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 8 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte08] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 8 del art�culo en el almac�n en el a�o indicado
	[nEntradas09] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 9 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte09] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 9 del art�culo en el almac�n en el a�o indicado
	[nEntradas10] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 10 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte10] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 10 del art�culo en el almac�n en el a�o indicado
	[nEntradas11] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 11 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte11] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 11 del art�culo en el almac�n en el a�o indicado
	[nEntradas12] decimal(18,4) NOT NULL,    -- cantidad de entradas aplicadas en el mes 12 del art�culo en el almac�n en el a�o indicado
	[nEntradasImporte12] decimal(18,4) NOT NULL,    -- valor de inventario de entradas aplicadas en el mes 12 del art�culo en el almac�n en el a�o indicado
	[nSalidas01] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 1 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte01] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 1 del art�culo en el almac�n en el a�o indicado
	[nSalidas02] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 2 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte02] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 2 del art�culo en el almac�n en el a�o indicado
	[nSalidas03] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 3 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte03] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 3 del art�culo en el almac�n en el a�o indicado
	[nSalidas04] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 4 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte04] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 4 del art�culo en el almac�n en el a�o indicado
	[nSalidas05] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 5 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte05] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 5 del art�culo en el almac�n en el a�o indicado
	[nSalidas06] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 6 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte06] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 6 del art�culo en el almac�n en el a�o indicado
	[nSalidas07] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 7 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte07] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 7 del art�culo en el almac�n en el a�o indicado
	[nSalidas08] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 8 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte08] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 8 del art�culo en el almac�n en el a�o indicado
	[nSalidas09] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 9 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte09] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 9 del art�culo en el almac�n en el a�o indicado
	[nSalidas10] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 10 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte10] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 10 del art�culo en el almac�n en el a�o indicado
	[nSalidas11] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 11 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte11] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 11 del art�culo en el almac�n en el a�o indicado
	[nSalidas12] decimal(18,4) NOT NULL,    -- cantidad de salidas aplicadas en el mes 12 del art�culo en el almac�n en el a�o indicado
	[nSalidasImporte12] decimal(18,4) NOT NULL,    -- valor de inventario de salidas aplicadas en el mes 12 del art�culo en el almac�n en el a�o indicado
	[nCantidadSaldoFinal] decimal(18,4) NOT NULL,    -- valor de la cantidad de existencia con la que va acumulando para terminar el art�culo en el almac�n en el a�o indicado, deberia ser el mismo que est� en la tabla de existencias en el campo nExistencia
	[nImporteSaldoFinal] decimal(18,4) NOT NULL,    -- valor de inventario con la que va acumlando para terminar el art�culo en el almac�n en el a�o indicado,, deberia ser el mismo que est� en la tabla de existencias en el campo nValorInventario
	[bActivo] bit NOT NULL,    -- campo que indica si est� activo o no el registro
	[cUsuario_Registra] varchar(50) NOT NULL,    -- login del usuario que realiza el registro
	[cMaquina_Registra] varchar(50) NOT NULL,    -- nombre del equipo desde donde se realiza el registro
	[dFecha_Registra] datetime NOT NULL,    -- fecha en que se hace el registro
	[cUsuario_Modifica] varchar(50) NULL,    -- login del usuario que realiza la ultima modificaci�n del registro
	[cMaquina_Modifica] varchar(50) NULL,    -- nombre del equipo desde donde se realiza la ultima modificaci�n del registro
	[dFecha_Modifica] datetime NULL,    -- fecha en que se hace la ultima modificaci�n del registro
	[cUsuario_Cancela] varchar(50) NULL,    -- login del usuario que realiza la ultima inactivaci�n del registro
	[cMaquina_Cancela] varchar(50) NULL,    -- nombre del equipo desde donde se realiza la ultima inactivaci�n del registro
	[dFecha_Cancela] datetime NULL    -- fecha en que se hace la ultima inactivaci�n del registro
)


/* Create Primary Keys, Indexes, Uniques, Checks */
	ALTER TABLE [IVT_ResumenSaldosExistencias] ADD CONSTRAINT [PK_IVT_ResumenSaldosExistencias]
	PRIMARY KEY CLUSTERED ([nAlmacen] ASC,[nIDArticulo] ASC)
END
GO

/* Create Foreign Key Constraints */
IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_IVT_ResumenSaldosExistencias_CAT_Almacenes' AND xtype = 'F')
	ALTER TABLE [IVT_ResumenSaldosExistencias] ADD CONSTRAINT [FK_IVT_ResumenSaldosExistencias_CAT_Almacenes]
	FOREIGN KEY ([nAlmacen]) REFERENCES [dbo].[CAT_Almacenes] ([nAlmacen]) ON DELETE No Action ON UPDATE No Action
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_IVT_ResumenSaldosExistencias_CAT_Articulos' AND xtype = 'F')
	ALTER TABLE [IVT_ResumenSaldosExistencias] ADD CONSTRAINT [FK_IVT_ResumenSaldosExistencias_CAT_Articulos]
	FOREIGN KEY ([nIDArticulo]) REFERENCES [CAT_Articulos] ([nIDArticulo]) ON DELETE No Action ON UPDATE No Action
GO
/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 16-may.-2024 22:38:32 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Drop Tables */

/* Create Tables */
IF NOT EXISTS (SELECT 1 FROM dbo.sysobjects WHERE id = object_id(N'[SAT_CatUsoCFDI]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) 
BEGIN
CREATE TABLE [dbo].[SAT_CatUsoCFDI]
(
	[UsoCFDI] varchar(255) NOT NULL,
	[Descripcion] varchar(255) NULL,
	[AplicaPersonaFisica] bit NULL,
	[AplicaPersonaMoral] bit NULL,
	[FechaInicioVigencia] datetime NULL,
	[RegimenFiscalReceptor] varchar(255) NULL
)


	/* Create Primary Keys, Indexes, Uniques, Checks */

	ALTER TABLE [dbo].[SAT_CatUsoCFDI] ADD CONSTRAINT [PK_SAT_CatUsoCFDI]
	PRIMARY KEY CLUSTERED ([UsoCFDI] ASC)
END
GO
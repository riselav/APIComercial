/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 13.0 		*/
/*  Created On : 09-feb.-2025 10:37:16 p. m. 				*/
/*  DBMS       : SQL Server 2012 						*/
/* ---------------------------------------------------- */

/* Create Tables */
IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='CAT_Catalogos' AND xtype = 'U')
BEGIN
CREATE TABLE [dbo].[CAT_Catalogos]
(
	[nCatalogo] int NOT NULL,    -- id
	[cNombre] varchar(50) NOT NULL,
	[nCodigo] int NOT NULL,
	[cDescripcion] varchar(150) NOT NULL,
	[bActivo] bit NOT NULL    -- identificador para saber si el registro se encuentra activo o no
)


/* Create Primary Keys, Indexes, Uniques, Checks */
	ALTER TABLE [dbo].[CAT_Catalogos] ADD CONSTRAINT [PK_CAT_Catalogos]
	PRIMARY KEY CLUSTERED ([nCatalogo] ASC)
END
GO
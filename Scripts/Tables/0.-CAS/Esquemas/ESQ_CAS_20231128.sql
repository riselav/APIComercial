IF [dbo].[fn_ExisteCampo]('CAT_OpcionesMenu','bFormaHija')=0
	ALTER TABLE CAT_OpcionesMenu ADD bFormaHija bit CONSTRAINT DF_FrmHj DEFAULT 1
GO

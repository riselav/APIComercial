IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='nTipo')
	ALTER TABLE CAT_Usuarios ADD nTipo tinyint
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cNombre')
	ALTER TABLE CAT_Usuarios ADD cNombre varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cApellidoPaterno')
	ALTER TABLE CAT_Usuarios ADD cApellidoPaterno varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cApellidoMaterno')
	ALTER TABLE CAT_Usuarios ADD cApellidoMaterno varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cUsuario_Registra')
	ALTER TABLE CAT_Usuarios ADD cUsuario_Registra varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cMaquina_Registra')
	ALTER TABLE CAT_Usuarios ADD cMaquina_Registra varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='dFecha_Registra')
	ALTER TABLE CAT_Usuarios ADD dFecha_Registra datetime
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cUsuario_Modifica')
	ALTER TABLE CAT_Usuarios ADD cUsuario_Modifica varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cMaquina_Modifica')
	ALTER TABLE CAT_Usuarios ADD cMaquina_Modifica varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='dFecha_Modifica')
	ALTER TABLE CAT_Usuarios ADD dFecha_Modifica datetime
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cUsuario_Cancela')
	ALTER TABLE CAT_Usuarios ADD cUsuario_Cancela varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='cMaquina_Cancela')
	ALTER TABLE CAT_Usuarios ADD cMaquina_Cancela varchar(50)
GO

IF NOT EXISTS( SELECT 1 FROM Information_Schema.Columns WHERE TABLE_NAME = 'CAT_Usuarios' AND COLUMN_NAME='dFecha_Cancela')
	ALTER TABLE CAT_Usuarios ADD dFecha_Cancela datetime
GO

-- *******************

IF NOT EXISTS(SELECT 1 FROM sys.indexes WHERE name='UQ_IDX_Tipo_Usuario')
	CREATE UNIQUE NONCLUSTERED INDEX UQ_IDX_Tipo_Usuario ON CAT_Usuarios(nTipo) WHERE nTipo =1
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name = 'UQ_Usuario_CAT_Usuarios' AND xtype = 'UQ')  
	ALTER TABLE dbo.CAT_Usuarios
	ADD CONSTRAINT UQ_Usuario_CAT_Usuarios UNIQUE (cUsuario)
GO
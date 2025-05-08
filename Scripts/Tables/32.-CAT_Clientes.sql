if dbo.FN_Existetabla ('CAT_Clientes')=0
Begin
	CREATE TABLE [dbo].[CAT_Clientes](
		[nCliente] [int] NOT NULL,
		[cNombreCompleto] [varchar](150) NOT NULL,
		[cCalle] [varchar](200) NULL,
		[cNumExt] [varchar](20) NULL,
		[cNumInt] [varchar](20) NULL,
		[cColonia] [varchar](20) NULL,
		[cCodigoPostal] [varchar](20) NULL,
		[bActivo] [bit] NOT NULL,
		CONSTRAINT [PK_CAT_Clientes] PRIMARY KEY CLUSTERED 
		(
		[nCliente] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]
End 

IF dbo.fn_ExisteCampo('CAT_Clientes','cTelefono')=0
	ALTER TABLE CAT_Clientes ADD cTelefono varchar(30)
GO

IF dbo.fn_ExisteCampo('CAT_Clientes','cSeniasParticulares')=0
	ALTER TABLE CAT_Clientes ADD cSeniasParticulares varchar(500)
GO

IF dbo.fn_ExisteCampo('CAT_Clientes','nSucursalRegistro')=0
	ALTER TABLE CAT_Clientes ADD nSucursalRegistro int
GO
--
IF dbo.fn_ExisteCampo('CAT_Clientes','cUsuario_Registra')=0
	ALTER TABLE CAT_Clientes ADD cUsuario_Registra varchar(50)
GO
IF dbo.fn_ExisteCampo('CAT_Clientes','cMaquina_Registra')=0
	ALTER TABLE CAT_Clientes ADD cMaquina_Registra varchar(50)
GO
IF dbo.fn_ExisteCampo('CAT_Clientes','dFecha_Registra')=0
	ALTER TABLE CAT_Clientes ADD dFecha_Registra datetime
GO

IF dbo.fn_ExisteCampo('CAT_Clientes','cUsuario_Modifica')=0
	ALTER TABLE CAT_Clientes ADD cUsuario_Modifica varchar(50)
GO
IF dbo.fn_ExisteCampo('CAT_Clientes','cMaquina_Modifica')=0
	ALTER TABLE CAT_Clientes ADD cMaquina_Modifica varchar(50)
GO
IF dbo.fn_ExisteCampo('CAT_Clientes','dFecha_Modifica')=0
	ALTER TABLE CAT_Clientes ADD dFecha_Modifica datetime
GO

IF dbo.fn_ExisteCampo('CAT_Clientes','cUsuario_Cancela')=0
	ALTER TABLE CAT_Clientes ADD cUsuario_Cancela varchar(50)
GO
IF dbo.fn_ExisteCampo('CAT_Clientes','cMaquina_Cancela')=0
	ALTER TABLE CAT_Clientes ADD cMaquina_Cancela varchar(50)
GO
IF dbo.fn_ExisteCampo('CAT_Clientes','dFecha_Cancela')=0
	ALTER TABLE CAT_Clientes ADD dFecha_Cancela datetime
GO

IF not EXISTS (SELECT 1 FROM SysObjects (NOLOCK) where name ='FK_CAT_Clientes_CAT_Sucursales' AND xtype = 'F')
	ALTER TABLE [CAT_Clientes] ADD CONSTRAINT [FK_CAT_Clientes_CAT_Sucursales]
	FOREIGN KEY ([nSucursalRegistro]) REFERENCES [CAT_Sucursales] ([nSucursal]) ON DELETE No Action ON UPDATE No Action
GO

IF dbo.fn_ExisteCampo ('CAT_Clientes', 'bManejaCredito')=0  
	ALTER TABLE CAT_Clientes ADD bManejaCredito bit not null CONSTRAINT DF_CAT_Clientes_bManejaCredito DEFAULT 0 
GO

IF dbo.fn_ExisteCampo ('CAT_Clientes', 'nTipoPersona')=0  
	ALTER TABLE CAT_Clientes ADD nTipoPersona tinyint
GO

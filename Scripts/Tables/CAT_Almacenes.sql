if dbo.FN_Existetabla ('CAT_Almacenes')=0  
Begin

CREATE TABLE [dbo].[CAT_Almacenes](
	[nAlmacen] [int] NOT NULL,
	[cDescripcion] [varchar](200) NOT NULL,
	[nPlaza] [int] NOT NULL,
	[nSucursal] [int] NOT NULL,
	[cCodigoPostal] [varchar](10) NOT NULL,
	[cColonia] [varchar](10) NOT NULL,
	[nZona] [int] NOT NULL,
	[cDomicilio] [varchar](300) NOT NULL,
	[cTelefono1] [varchar](20) NOT NULL,
	[cTelefono2] [varchar](20) NOT NULL,
	[bActivo] [bit] NOT NULL,
	[cUsuario_Registra] [varchar](50) NULL,
	[cMaquina_Registra] [varchar](50) NULL,
	[dFecha_Registra] [smalldatetime] NULL,
	[cUsuario_Modifica] [varchar](50) NULL,
	[cMaquina_Modifica] [varchar](50) NULL,
	[dFecha_Modifica] [varchar](50) NULL,
	[cUsuario_Cancela] [varchar](50) NULL,
	[cMaquina_Cancela] [varchar](50) NULL,
	[dFecha_Cancela] [smalldatetime] NULL,
 CONSTRAINT [PK_CAT_Almacenes] PRIMARY KEY CLUSTERED 
(
	[nAlmacen] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


End 

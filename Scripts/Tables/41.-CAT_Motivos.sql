IF DBO.FN_Existetabla ('CAT_Motivos') =0 
BEGIN
CREATE TABLE [dbo].[CAT_Motivos](
	[nMotivo] [int] NOT NULL,
	[cDescripcion] [varchar](50) NOT NULL,
	[nTipoMotivo] [int] NOT NULL,
	[bActivo] [bit] NOT NULL,
	[cMaquina_Registra] [varchar](50) NULL,
	[cUsuario_Registra] [varchar](50) NULL,
	[dFecha_Registra] [smalldatetime] NULL,
	[cMaquina_Modifica] [varchar](50) NULL,
	[cUsuario_Modifica] [varchar](50) NULL,
	[dFecha_Modifica] [smalldatetime] NULL,
	[cMaquina_Cancela] [varchar](50) NULL,
	[cUsuario_Cancela] [varchar](50) NULL,
	[dFecha_Cancela] [smalldatetime] NULL,
 CONSTRAINT [PK_CAT_Motivos] PRIMARY KEY CLUSTERED 
(
	[nMotivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

END 



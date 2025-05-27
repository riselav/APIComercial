if dbo.FN_Existetabla ('CAT_Catalogos')=0
Begin

CREATE TABLE [dbo].[CAT_Catalogos](
	[nCatalogo] [int] NOT NULL,
	[cNombre] [varchar](50) NOT NULL,
	[nCodigo] [int] NOT NULL,
	[cDescripcion] [varchar](150) NOT NULL,
	[bActivo] [bit] NOT NULL,
 CONSTRAINT [PK_CAT_Catalogos] PRIMARY KEY CLUSTERED 
(
	[nCatalogo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
End 


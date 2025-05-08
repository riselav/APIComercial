
INSERT INTO [dbo].[CAT_Catalogos]
           ([nCatalogo]
           ,[cNombre]
           ,[nCodigo]
           ,[cDescripcion]
           ,[bActivo])
     VALUES
           ((select COALESCE(max(nCatalogo),0)+1 from CAT_Catalogos(nolock))
           ,'CAT_TIPOS_ARTICULO'
           ,1
           ,'INDIVIDUAL'
           ,1)
GO
INSERT INTO [dbo].[CAT_Catalogos]
           ([nCatalogo]
           ,[cNombre]
           ,[nCodigo]
           ,[cDescripcion]
           ,[bActivo])
     VALUES
           ((select COALESCE(max(nCatalogo),0)+1 from CAT_Catalogos(nolock))
           ,'CAT_TIPOS_ARTICULO'
           ,2
           ,'KIT'
           ,1)
GO
INSERT INTO [dbo].[CAT_Catalogos]
           ([nCatalogo]
           ,[cNombre]
           ,[nCodigo]
           ,[cDescripcion]
           ,[bActivo])
     VALUES
           ((select COALESCE(max(nCatalogo),0)+1 from CAT_Catalogos(nolock))
           ,'CAT_TIPOS_ARTICULO'
           ,3
           ,'PAQUETE'
           ,1)
GO


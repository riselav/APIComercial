--select * from CAT_Catalogos
INSERT INTO [dbo].[CAT_Catalogos]
           ([nCatalogo]
           ,[cNombre]
           ,[nCodigo]
           ,[cDescripcion]
           ,[bActivo])
     VALUES
           ((select COALESCE(max(nCatalogo),0)+1 from CAT_Catalogos(nolock))
           ,'CAT_TIPO_UNIDAD'
           ,1
           ,'PESO'
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
           ,'CAT_TIPO_UNIDAD'
           ,2
           ,'LIQUIDO'
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
           ,'CAT_TIPO_UNIDAD'
           ,3
           ,'PEIZA'
           ,1)
GO




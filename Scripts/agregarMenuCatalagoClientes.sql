INSERT INTO [dbo].[CAT_OpcionesMenu]
           ([nOpcion]
           ,[cDescripcion]
           ,[nModulo]
           ,[cFormulario]
           ,[cComponenteInstancia]
           ,[nOpcionAgrupador]
           ,[nOrden]
           ,[bActivo]
           ,[cMaquina_Registra]
           ,[cUsuario_Registra]
           ,[dFecha_Registra]
           ,[cMaquina_Modifica]
           ,[cUsuario_Modifica]
           ,[dFecha_Modifica]
           ,[cMaquina_Cancela]
           ,[cRegistro_Cancela]
           ,[dFecha_Cancela]
           ,[bFormaHija])
     VALUES
           ((select COALESCE(max(nOpcion),0)+1 from CAT_OpcionesMenu(nolock))
           ,'Catalago Clientes'
           ,2
           ,''
           ,''
           ,null
           ,null
           ,1
           ,'admin'
           ,'admin'
           ,getdate()
           ,null
           ,null
           ,null
           ,null
           ,null
           ,null
           ,null)
GO

INSERT INTO [dbo].[CAT_PermisosMenu]
           ([nPermiso]
           ,[nOpcion]
           ,[nPerfil]
           ,[nUsuario]
           ,[bActivo]
           ,[cMaquina_Registra]
           ,[cUsuario_Registra]
           ,[dFecha_Registra]
           ,[cMaquina_Modifica]
           ,[cUsuario_Modifica]
           ,[dFecha_Modifica]
           ,[cMaquina_Cancela]
           ,[cRegistro_Cancela]
           ,[dFecha_Cancela])
     VALUES
           ((select COALESCE(max(nPermiso),0)+1 from CAT_PermisosMenu (nolock))
           ,(select COALESCE(max(nOpcion),0) from CAT_OpcionesMenu(nolock))
           ,5
           ,2
           ,1
           ,'admin'
           ,'admin'
           ,getdate()
           ,null
           ,null
           ,null
           ,null
           ,null
           ,null)
GO
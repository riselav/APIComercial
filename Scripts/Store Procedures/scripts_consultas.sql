SELECT * FROM CAT_OpcionesMenu (NOLOCK)

select * from CAT_Lineas

select * from CAT_Marcas

select * from CAT_Unidades

select * from CAT_Sublineas

SELECT * FROM CAT_OpcionesMenu (NOLOCK) where nModulo = 2 order by nOpcion desc


SELECT * FROM CAT_OpcionesMenu (NOLOCK) order by nOpcion desc

SELECT [nPerfil]
      ,[cDescripcion]
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
  FROM [dbo].[CAT_Perfiles]


SELECT * FROM CAT_ModulosMenu (NOLOCK)
--Este catalogo contiene los nombres de los modulos con los que va a contar el sistema

SELECT * FROM CAT_Usuarios (NOLOCK)
--Esta es la tabla que guarda los usuarios, del sistema, estos están vinculados por un empleado previamente guardado y la contraseña se encuentra encriptada.

SELECT * FROM CAT_OpcionesMenu (NOLOCK)
 

SELECT * FROM CAT_PermisosMenu (NOLOCK)
--Esta tabla almacena ya los permisos a los que tendrá derecho cada usuario.

SELECT * FROM CAT_OperacionesRestringidas (NOLOCK)
--Esta tabla guarda todas las operaciones o funciones especiales que se manejaran con autorización de personal autorizado.

SELECT * FROM CAT_PermisosOperacionesRestringidas (NOLOCK)
--Esta tabla guarda que usuarios serán los supervisiones de la funciones u operaciones especiales; es decir, solo estos usuarios podrán realizar dichas funciones, en caso contrario se le pedirá autorización previa.

 

 select * from CAT_OpcionesMenu where nOpcion=30
USE [Voalaft_Navola]
GO

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
           ,'Catalago Marcas'
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
select * from [CAT_PermisosMenu]

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
           ,30
           ,5
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
           ,null)
GO



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
           ,'Catalago Sublineas'
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
select * from [CAT_PermisosMenu]

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
           ,31
           ,5
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
           ,null)
GO




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
           ,'Catalago Unidades'
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
select * from [CAT_PermisosMenu]
select * from [CAT_OpcionesMenu]

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
           ,32
           ,5
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
           ,null)
GO


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
           ,'Catalago Lineas'
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
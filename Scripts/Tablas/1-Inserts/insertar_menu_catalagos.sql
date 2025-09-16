/*ALTER TABLE CAT_OpcionesMenu ADD bWeb bit

alter table CAT_ModulosMenu add cIcono varchar(50) null;
 
update CAT_ModulosMenu set cIcono='Settings' where nModulo=2;
update CAT_ModulosMenu set cIcono='Security' where nModulo=1;
update CAT_ModulosMenu set cIcono='Store' where nModulo=4;
*/

-- begin tran
-- rollback tran
begin 
 declare @nOpcionCat int = 0;

 select @nOpcionCat = COALESCE(max(nOpcion),0)+1 from CAT_OpcionesMenu(nolock);


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
           ,[bFormaHija],
		   bWeb)
     VALUES
           (@nOpcionCat,'Catálogos',2
           ,'Source' -- en [cFormulario] pondremos el icono en caso de que tenga la opcion
           ,''  -- en [cComponenteInstancia] pondremos el nombre del componente
           ,null -- en [nOpcionAgrupador] pondremos el nodo padre cuando sea null el padre sera el modulo
           ,1000,1,'admin','admin',getdate(),null,null,null,null,null,null,null,1);
		   
	insert into CAT_AgrupadoresMenu (nAgrupador,cDescripcion,bActivo,cMaquina_Registra,cUsuario_Registra,dFecha_Registra)
	values (@nOpcionCat,'Catálogos',1,'admin','admin',getdate());
end


go

begin

    -- Declaramos la variable que contendrá el ID del menú padre ('Catálogos')
    declare @nOpcionPadre int = 0;

    -- Obtenemos el ID del menú 'Catálogos' que insertamos en el primer bloque
    select @nOpcionPadre = COALESCE(max(nOpcion),0) from CAT_OpcionesMenu(nolock) where cDescripcion = 'Catálogos';
    
    -- Declaramos una variable para el nuevo ID inicial
    declare @nOpcionInicial int = 0;
    
    -- Obtenemos el máximo ID global y le sumamos 1 para empezar a insertar desde ahí
    select @nOpcionInicial = COALESCE(max(nOpcion),0) + 1 from CAT_OpcionesMenu(nolock);


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
               ,[bWeb])
         VALUES
                (@nOpcionInicial,'Lineas',2,null,'LineasBuzon',@nOpcionPadre,1010,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+1,'Articulos',2,null,'PageCatArticulos',@nOpcionPadre,1020,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+2,'Clientes',2,null,'PageCatClientes',@nOpcionPadre,1030,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+3,'Grupo Empresarial',2,null,'GrupoEmpresarialBuzon',@nOpcionPadre,1040,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+4,'Impuestos',2,null,'PageCatImpuestos',@nOpcionPadre,1050,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+5,'Unidades',2,null,'UnidadesBuzon',@nOpcionPadre,1060,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+6,'Marcas',2,null,'MarcasBuzon',@nOpcionPadre,1070,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+7,'Sublineas',2,null,'SublineasBuzon',@nOpcionPadre,1080,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+8,'Empresas',2,null,'EmpresasBuzon',@nOpcionPadre,1090,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+9,'Productos Base',2,null,'PageCatProductosBase',@nOpcionPadre,1100,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+10,'Unidad Relacional',2,null,'PageCatUnidadesRelacionales',@nOpcionPadre,1110,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+11,'Proveedores',2,null,'ProveedoresBuzon',@nOpcionPadre,1120,1,'admin','admin',getdate(),1)
               ,(@nOpcionInicial+13,'Cajas',2,null,'CajasTablero',@nOpcionPadre,1140,1,'admin','admin',getdate(),1)
               ;
end
GO
BEGIN
	-- --- Configuración ---
	-- Modifica estas variables según tus necesidades
	DECLARE @nPerfilID INT = 5; -- ID del Perfil al que se le darán los permisos (ej. Administradores)
	DECLARE @nUsuarioID INT = NULL; -- Opcional: Asignar a un usuario específico. En tu ejemplo era 2. Déjalo en NULL si es solo por perfil.
	DECLARE @cUsuarioMaquina VARCHAR(50) = 'admin'; -- Usuario/Máquina que ejecuta el script
	-- ---------------------

	DECLARE @nOpcionPadre INT;

	-- 1. Buscamos el ID del menú padre 'Catálogos' para identificar a sus hijos.
	SELECT @nOpcionPadre = nOpcion
	FROM dbo.CAT_OpcionesMenu (NOLOCK)
	WHERE cDescripcion = 'Catálogos' AND nOpcionAgrupador IS NULL;

	IF @nOpcionPadre IS NULL
	BEGIN
		PRINT 'No se encontró el menú padre "Catálogos". No se insertaron permisos.';
		RETURN;
	END

	-- 2. Insertamos los permisos para el menú padre y todas sus opciones hijas.
	INSERT INTO [dbo].[CAT_PermisosMenu]
			   ([nPermiso], [nOpcion], [nPerfil], [nUsuario], [bActivo], [cMaquina_Registra], [cUsuario_Registra], [dFecha_Registra])
	SELECT
		(SELECT COALESCE(MAX(nPermiso), 0) FROM dbo.CAT_PermisosMenu (NOLOCK)) + ROW_NUMBER() OVER (ORDER BY Opciones.nOpcion ASC),
		
		Opciones.nOpcion,
		
		@nPerfilID,
		
		@nUsuarioID,
		
		1, -- bActivo
		@cUsuarioMaquina, -- cMaquina_Registra
		@cUsuarioMaquina, -- cUsuario_Registra
		GETDATE()         -- dFecha_Registra
	FROM
		dbo.CAT_OpcionesMenu AS Opciones (NOLOCK)
	WHERE
		(Opciones.nOpcion = @nOpcionPadre OR Opciones.nOpcionAgrupador = @nOpcionPadre)
		
		-- Condición 2: Nos aseguramos de no insertar un permiso que ya exista para ese perfil
		AND NOT EXISTS (
			SELECT 1
			FROM dbo.CAT_PermisosMenu AS PermisosExistentes (NOLOCK)
			WHERE PermisosExistentes.nOpcion = Opciones.nOpcion
			  AND PermisosExistentes.nPerfil = @nPerfilID
		);

	PRINT CAST(@@ROWCOUNT AS VARCHAR) + ' permisos nuevos fueron insertados para el Perfil ID: ' + CAST(@nPerfilID AS VARCHAR);

END
GO


begin 
 declare @nOpcionCat int = 0;

 select @nOpcionCat = COALESCE(max(nOpcion),0)+1 from CAT_OpcionesMenu(nolock);


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
           ,[bFormaHija],
		   bWeb)
     VALUES
           (@nOpcionCat,'Ventas',4
           ,'ShoppingCart' -- en [cFormulario] pondremos el icono en caso de que tenga la opcion
           ,''  -- en [cComponenteInstancia] pondremos el nombre del componente
           ,null -- en [nOpcionAgrupador] pondremos el nodo padre cuando sea null el padre sera el modulo
           ,4000,1,'admin','admin',getdate(),null,null,null,null,null,null,null,1);
		   
	insert into CAT_AgrupadoresMenu (nAgrupador,cDescripcion,bActivo,cMaquina_Registra,cUsuario_Registra,dFecha_Registra)
	values (@nOpcionCat,'Ventas',1,'admin','admin',getdate());
end


go


begin

    -- Declaramos la variable que contendrá el ID del menú padre ('Catálogos')
    declare @nOpcionPadre int = 0;

    -- Obtenemos el ID del menú 'Catálogos' que insertamos en el primer bloque
    select @nOpcionPadre = COALESCE(max(nOpcion),0) from CAT_OpcionesMenu(nolock) where cDescripcion = 'Ventas';
    
    -- Declaramos una variable para el nuevo ID inicial
    declare @nOpcionInicial int = 0;
    
    -- Obtenemos el máximo ID global y le sumamos 1 para empezar a insertar desde ahí
    select @nOpcionInicial = COALESCE(max(nOpcion),0) + 1 from CAT_OpcionesMenu(nolock);


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
               ,[bWeb])
         VALUES
                (@nOpcionInicial,'Registro de Venta',4,'AddShoppingCart','RegistroVenta',@nOpcionPadre,4010,1,'admin','admin',getdate(),1),
				(@nOpcionInicial+1,'Reporte de Ventas',4,'Insights','ReporteVentas',@nOpcionPadre,4020,1,'admin','admin',getdate(),1)
               ;
			   
end
GO
BEGIN
	-- --- Configuración ---
	-- Modifica estas variables según tus necesidades
	DECLARE @nPerfilID INT = 5; -- ID del Perfil al que se le darán los permisos (ej. Administradores)
	DECLARE @nUsuarioID INT = NULL; -- Opcional: Asignar a un usuario específico. En tu ejemplo era 2. Déjalo en NULL si es solo por perfil.
	DECLARE @cUsuarioMaquina VARCHAR(50) = 'admin'; -- Usuario/Máquina que ejecuta el script
	-- ---------------------

	DECLARE @nOpcionPadre INT;

	-- 1. Buscamos el ID del menú padre 'Catálogos' para identificar a sus hijos.
	SELECT @nOpcionPadre = nOpcion
	FROM dbo.CAT_OpcionesMenu (NOLOCK)
	WHERE cDescripcion = 'Ventas' AND nOpcionAgrupador IS NULL;

	IF @nOpcionPadre IS NULL
	BEGIN
		PRINT 'No se encontró el menú padre "Catálogos". No se insertaron permisos.';
		RETURN;
	END

	-- 2. Insertamos los permisos para el menú padre y todas sus opciones hijas.
	INSERT INTO [dbo].[CAT_PermisosMenu]
			   ([nPermiso], [nOpcion], [nPerfil], [nUsuario], [bActivo], [cMaquina_Registra], [cUsuario_Registra], [dFecha_Registra])
	SELECT
		(SELECT COALESCE(MAX(nPermiso), 0) FROM dbo.CAT_PermisosMenu (NOLOCK)) + ROW_NUMBER() OVER (ORDER BY Opciones.nOpcion ASC),
		
		Opciones.nOpcion,
		
		@nPerfilID,
		
		@nUsuarioID,
		
		1, -- bActivo
		@cUsuarioMaquina, -- cMaquina_Registra
		@cUsuarioMaquina, -- cUsuario_Registra
		GETDATE()         -- dFecha_Registra
	FROM
		dbo.CAT_OpcionesMenu AS Opciones (NOLOCK)
	WHERE
		(Opciones.nOpcion = @nOpcionPadre OR Opciones.nOpcionAgrupador = @nOpcionPadre)
		
		-- Condición 2: Nos aseguramos de no insertar un permiso que ya exista para ese perfil
		AND NOT EXISTS (
			SELECT 1
			FROM dbo.CAT_PermisosMenu AS PermisosExistentes (NOLOCK)
			WHERE PermisosExistentes.nOpcion = Opciones.nOpcion
			  AND PermisosExistentes.nPerfil = @nPerfilID
		);

	PRINT CAST(@@ROWCOUNT AS VARCHAR) + ' permisos nuevos fueron insertados para el Perfil ID: ' + CAST(@nPerfilID AS VARCHAR);

END
GO






begin 
 declare @nOpcionCat int = 0;

 select @nOpcionCat = COALESCE(max(nOpcion),0)+1 from CAT_OpcionesMenu(nolock);


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
           ,[bFormaHija],
		   bWeb)
     VALUES
           (@nOpcionCat,'Caja',4
           ,'PointOfSale' -- en [cFormulario] pondremos el icono en caso de que tenga la opcion
           ,''  -- en [cComponenteInstancia] pondremos el nombre del componente
           ,null -- en [nOpcionAgrupador] pondremos el nodo padre cuando sea null el padre sera el modulo
           ,4100,1,'admin','admin',getdate(),null,null,null,null,null,null,null,1);
		   
	insert into CAT_AgrupadoresMenu (nAgrupador,cDescripcion,bActivo,cMaquina_Registra,cUsuario_Registra,dFecha_Registra)
	values (@nOpcionCat,'Caja',1,'admin','admin',getdate());
end


go

begin

    -- Declaramos la variable que contendrá el ID del menú padre ('Catálogos')
    declare @nOpcionPadre int = 0;

    -- Obtenemos el ID del menú 'Catálogos' que insertamos en el primer bloque
    select @nOpcionPadre = COALESCE(max(nOpcion),0) from CAT_OpcionesMenu(nolock) where cDescripcion = 'Caja';
    
    -- Declaramos una variable para el nuevo ID inicial
    declare @nOpcionInicial int = 0;
    
    -- Obtenemos el máximo ID global y le sumamos 1 para empezar a insertar desde ahí
    select @nOpcionInicial = COALESCE(max(nOpcion),0) + 1 from CAT_OpcionesMenu(nolock);


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
               ,[bWeb])
         VALUES
                (@nOpcionInicial,'Apertura de Caja',4,null,'AperturaCaja',@nOpcionPadre,4110,1,'admin','admin',getdate(),1),
				(@nOpcionInicial + 1,'Registro Corte de Caja',4,null,'RegistroCorteCaja',@nOpcionPadre,4120,1,'admin','admin',getdate(),1),
				(@nOpcionInicial + 2,'Ingresos',4,null,'IngresoCajaScreen',@nOpcionPadre,4130,1,'admin','admin',getdate(),1),
				(@nOpcionInicial + 3,'Egresos',4,null,'EgresoCajaScreen',@nOpcionPadre,4140,1,'admin','admin',getdate(),1),
				(@nOpcionInicial + 4,'Retiros',4,null,'RetiroCajaScreen',@nOpcionPadre,4150,1,'admin','admin',getdate(),1)
               ;
end
GO
BEGIN
	-- --- Configuración ---
	-- Modifica estas variables según tus necesidades
	DECLARE @nPerfilID INT = 5; -- ID del Perfil al que se le darán los permisos (ej. Administradores)
	DECLARE @nUsuarioID INT = NULL; -- Opcional: Asignar a un usuario específico. En tu ejemplo era 2. Déjalo en NULL si es solo por perfil.
	DECLARE @cUsuarioMaquina VARCHAR(50) = 'admin'; -- Usuario/Máquina que ejecuta el script
	-- ---------------------

	DECLARE @nOpcionPadre INT;

	-- 1. Buscamos el ID del menú padre 'Catálogos' para identificar a sus hijos.
	SELECT @nOpcionPadre = nOpcion
	FROM dbo.CAT_OpcionesMenu (NOLOCK)
	WHERE cDescripcion = 'Caja' AND nOpcionAgrupador IS NULL;

	IF @nOpcionPadre IS NULL
	BEGIN
		PRINT 'No se encontró el menú padre "Catálogos". No se insertaron permisos.';
		RETURN;
	END

	-- 2. Insertamos los permisos para el menú padre y todas sus opciones hijas.
	INSERT INTO [dbo].[CAT_PermisosMenu]
			   ([nPermiso], [nOpcion], [nPerfil], [nUsuario], [bActivo], [cMaquina_Registra], [cUsuario_Registra], [dFecha_Registra])
	SELECT
		(SELECT COALESCE(MAX(nPermiso), 0) FROM dbo.CAT_PermisosMenu (NOLOCK)) + ROW_NUMBER() OVER (ORDER BY Opciones.nOpcion ASC),
		
		Opciones.nOpcion,
		
		@nPerfilID,
		
		@nUsuarioID,
		
		1, -- bActivo
		@cUsuarioMaquina, -- cMaquina_Registra
		@cUsuarioMaquina, -- cUsuario_Registra
		GETDATE()         -- dFecha_Registra
	FROM
		dbo.CAT_OpcionesMenu AS Opciones (NOLOCK)
	WHERE
		(Opciones.nOpcion = @nOpcionPadre OR Opciones.nOpcionAgrupador = @nOpcionPadre)
		
		-- Condición 2: Nos aseguramos de no insertar un permiso que ya exista para ese perfil
		AND NOT EXISTS (
			SELECT 1
			FROM dbo.CAT_PermisosMenu AS PermisosExistentes (NOLOCK)
			WHERE PermisosExistentes.nOpcion = Opciones.nOpcion
			  AND PermisosExistentes.nPerfil = @nPerfilID
		);

	PRINT CAST(@@ROWCOUNT AS VARCHAR) + ' permisos nuevos fueron insertados para el Perfil ID: ' + CAST(@nPerfilID AS VARCHAR);

END
GO



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
                (@nOpcionInicial,'Sucursales',2,null,'SucursalesTablero',@nOpcionPadre,1050,1,'admin','admin',getdate(),1)
end
GO


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
                (@nOpcionInicial,'Tablero de Indicadores',4,NULL,'Dashboard',@nOpcionPadre,4030,1,'admin','admin',getdate(),1)
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
		(Opciones.cDescripcion = 'Sucursales')
		
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
		(Opciones.cDescripcion='Tablero de Indicadores')
		
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


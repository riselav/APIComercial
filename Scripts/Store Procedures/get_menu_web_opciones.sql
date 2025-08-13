-- get_menu_web_opciones 2
CREATE OR ALTER PROCEDURE get_menu_web_opciones
    @user_id INT
AS
BEGIN
	
    DECLARE @ESADMIN BIT = 0;
	DECLARE @Opciones as table(
	MENU_ID int,
	MENU_ID_PARENT int,
	MENU_DESCRIPCION_PARENT varchar(200),
	MENU_ORDEN_PARENT int,
	MENU_DESCRIPCION varchar(200),
	MENU_ORDEN int,
	menu_url text,
	MENU_ICONO text,
	MENU_ICONO_PARENT text
)

DROP TABLE IF EXISTS #tmpopcionesusuario;

    SELECT
        @ESADMIN = SP.bAdministrador
    FROM
        CAT_Usuarios AS SP
    WHERE
        sp.nFolio = @user_id;

	select distinct mm.nModulo,mm.cDescripcion as moduloDescripcion,om.nOpcion, om.cDescripcion as opcionDescripcion 
					, om.cFormulario as icono
					,om.cComponenteInstancia
					,om.nOpcionAgrupador
					,om.nOrden
					,mm.cIcono INTO #tmpopcionesusuario
                     from CAT_Usuarios u (nolock) 
                     inner join CAT_PerfilesUsuarios pu (nolock) on u.nFolio = pu.nUsuario 
                     inner join CAT_Perfiles(nolock)per on per.nPerfil = pu.nPerfil 
                     inner join CAT_PermisosMenu (nolock)pm on pm.nPerfil = pu.nPerfil 
                     inner join CAT_OpcionesMenu (nolock)om on om.nOpcion = pm.nOpcion 
                     inner join CAT_ModulosMenu (nolock)mm on mm.nModulo = om.nModulo 
                     where (u.nFolio = 2 or @ESADMIN=1) and u.bActivo = 1 and pu.bActivo = 1 and per.bActivo = 1 and pm.bActivo = 1 and om.bActivo = 1 and mm.bActivo = 1 and om.bWeb=1
                     order by mm.nModulo,om.nOpcionAgrupador, om.nOpcion, om.nOrden;

	--INSERT INTO @Opciones (
	--MENU_ID,
	--MENU_DESCRIPCION ,
	--MENU_ORDEN )
	--SELECT distinct nModulo,moduloDescripcion,nModulo from #tmpopcionesusuario;

	INSERT INTO @Opciones (
	MENU_ID ,
	MENU_ID_PARENT ,
	MENU_DESCRIPCION_PARENT,
	MENU_ORDEN_PARENT ,
	MENU_DESCRIPCION,
	MENU_ORDEN ,
	menu_url ,
	MENU_ICONO,
	MENU_ICONO_PARENT)
	SELECT distinct nOpcion,nModulo,moduloDescripcion,nModulo,opcionDescripcion,nOrden,cComponenteInstancia,icono,cIcono from #tmpopcionesusuario where nOpcionAgrupador is null;

	INSERT INTO @Opciones (
	MENU_ID ,
	MENU_ID_PARENT ,
	MENU_DESCRIPCION_PARENT,
	MENU_ORDEN_PARENT ,
	MENU_DESCRIPCION,
	MENU_ORDEN ,
	menu_url ,
	MENU_ICONO,
	MENU_ICONO_PARENT)
	SELECT distinct op.nOpcion,padre.nOpcion,padre.opcionDescripcion,padre.nOrden,op.opcionDescripcion,op.nOrden,op.cComponenteInstancia,op.icono,op.icono 
	from #tmpopcionesusuario op inner join #tmpopcionesusuario padre on op.nOpcionAgrupador=padre.nOpcion where op.nOpcionAgrupador is NOT null;

	select MENU_ID ,
	MENU_ID_PARENT ,
	MENU_DESCRIPCION_PARENT,
	MENU_ORDEN_PARENT ,
	MENU_DESCRIPCION,
	MENU_ORDEN ,
	menu_url ,
	MENU_ICONO,
	MENU_ICONO_PARENT from @Opciones;

end

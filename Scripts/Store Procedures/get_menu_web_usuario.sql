CREATE OR ALTER PROCEDURE get_menu_web_usuario
    @user_id INT
AS
BEGIN
    DECLARE @ESADMIN BIT = 0;

    SELECT
        @ESADMIN = SP.bAdministrador
    FROM
        CAT_Usuarios AS SP
    WHERE
        sp.nFolio = @user_id;

    WITH cte AS
    (
        SELECT
            SM.MENU_ID,
            SM.MENU_ID AS MENU_ID_PARENT
        FROM
            cat_menu_web AS SM
        WHERE
            SM.MENU_ID_PARENT IS NULL
        UNION ALL
        SELECT
            SM.MENU_ID,
            SM.MENU_ID_PARENT
        FROM
            cte AS C
        INNER JOIN
            cat_menu_web SM ON C.MENU_ID = SM.MENU_ID_PARENT
    )
    SELECT
        c.MENU_ID,
        c.MENU_ID_PARENT,
        sm.MENU_DESCRIPCION AS MENU_DESCRIPCION_PARENT,
        SM.MENU_ORDEN AS MENU_ORDEN_PARENT,
        SM2.MENU_DESCRIPCION,
        SM2.MENU_ORDEN,
        SM2.MENU_URL,
        SM2.MENU_ICONO
    FROM
        cte AS C
    INNER JOIN
        cat_menu_web AS SM ON (c.MENU_ID_PARENT = SM.MENU_ID OR c.MENU_ID_PARENT IS NULL) AND SM.MENU_ESTATUS = 1
    INNER JOIN
        cat_menu_web AS SM2 ON (C.MENU_ID = SM2.MENU_ID) AND SM2.MENU_ESTATUS = 1
    WHERE
        (c.MENU_ID != c.MENU_ID_PARENT OR SM.MENU_ID_PARENT IS NULL) AND
        (
            @ESADMIN = 1 
            OR EXISTS (
                SELECT 1
                FROM cat_permisos_menu_web SPP2
                WHERE SPP2.MENU_ID = SM2.MENU_ID and SPP2.Activo=1
                  AND SPP2.nPerfil IN (SELECT nPerfil FROM CAT_PerfilesUsuarios WHERE nUsuario = @user_id and bActivo=1)
            )
        )
    ORDER BY
        SM2.MENU_ORDEN;
END;
go
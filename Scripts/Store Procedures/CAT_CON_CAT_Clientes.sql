--RST_CON_CAT_Clientes
SP_ELIMINASTORE 'CAT_CON_CAT_Clientes'
GO
CREATE PROCEDURE CAT_CON_CAT_Clientes (@nFolio bigint=0 )  
AS  
BEGIN  
 -- CAT_CON_CAT_Clientes 1

	SELECT EM.nCliente,ISNULL(CR.cRazonSocial,EM.cNombreCompleto) as cCliente, --EM.cNombreCompleto as cCliente,
	EM.cColonia,EM.cCodigoPostal,EM.cCalle,EM.cNumExt,EM.cNumInt,EM.bActivo,
	CP.cEstado,CP.cMunicipio,Mn.cNombreMunicipio,CP.cLocalidad,LC.cDescripcion as cNombreLocalidad,
	EM.cTelefono,EM.cSeniasParticulares,EM.nSucursalRegistro,EM.nIDRFC,EM.nTipoPersona,
	ISNULL(CR.cRFC,'') as cRFC, CR.nIDRFC,CR.cRazonSocial
	FROM CAT_Clientes EM (NOLOCK)
	LEFT JOIN CAT_RFC CR (NOLOCK) ON CR.nIDRFC=EM.nIDRFC
	LEFT JOIN CAT_Colonias CL (NOLOCK) ON CL.cColonia=Em.cColonia
	AND CL.cCodigoPostal=EM.cCodigoPostal
	LEFT JOIN CAT_CodigosPostales CP (NOLOCK) ON CP.cCodigoPostal=EM.cCodigoPostal
	LEFT JOIN CAT_Municipios Mn (NOLOCK) ON CP.cEstado=Mn.cEstado
	AND CP.cMunicipio=Mn.cMunicipio
	LEFT JOIN CAT_Localidades Lc (NOLOCK) ON CP.cEstado=Lc.cEstado
	AND CP.cLocalidad=Lc.cLocalidad
	WHERE EM.nCliente = CASE WHEN @nFolio=0 THEN EM.nCliente ELSE @nFolio END
	ORDER BY EM.dFecha_Registra

	IF @nFolio>0
	BEGIN
		SELECT nCliente,nContacto,C.cNombre,cPuesto,ISNULL(cTelefono,'') as cTelefono,cCelular,
		cCorreoElectronico,nTipoContacto,ISNULL(CC.cDescripcion,'') as cTipoContacto
		FROM CAT_ClientesContactos C(NOLOCK)
		LEFT JOIN CAT_Catalogos CC (NOLOCK) ON CC.nCodigo=C.nTipoContacto
			AND CC.cNombre='CAT_TipoContactoCliente'
		WHERE nCliente=@nFolio

		SELECT CC.nIDRFC as IDRFC,nFolio as Folio,cCorreoElectronico as CorreoElectronico,
		CC.bActivo as Activo,CC.cUsuario_Registra as Usuario,CC.cMaquina_Registra as Maquina
		FROM CAT_CorreosContactoRFC CC (NOLOCK)
		JOIN CAT_Clientes Cl (NOLOCK) ON CC.nIDRFC=Cl.nIDRFC
		WHERE nCliente=@nFolio
	END
END 
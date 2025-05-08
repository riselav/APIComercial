--RST_CON_CAT_Clientes
SP_ELIMINASTORE 'CAT_CON_CAT_Clientes'
GO
CREATE PROCEDURE CAT_CON_CAT_Clientes (@nFolio bigint=0 )  
AS  
BEGIN  
 -- CAT_CON_CAT_Clientes 1001000002

 SELECT EM.nCliente,EM.cNombreCompleto as cCliente,
 EM.cColonia,EM.cCodigoPostal,EM.cCalle,EM.cNumExt,EM.cNumInt,EM.bActivo,
 CP.cEstado,CP.cMunicipio,Mn.cNombreMunicipio,CP.cLocalidad,LC.cDescripcion as cNombreLocalidad,
 EM.cTelefono,EM.cSeniasParticulares,EM.nSucursalRegistro
 FROM CAT_Clientes EM (NOLOCK)
 LEFT JOIN CAT_Colonias CL (NOLOCK) ON CL.cColonia=Em.cColonia
	AND CL.cCodigoPostal=EM.cCodigoPostal
 LEFT JOIN CAT_CodigosPostales CP (NOLOCK) ON CP.cCodigoPostal=EM.cCodigoPostal
 LEFT JOIN CAT_Municipios Mn (NOLOCK) ON CP.cEstado=Mn.cEstado
	AND CP.cMunicipio=Mn.cMunicipio
 LEFT JOIN CAT_Localidades Lc (NOLOCK) ON CP.cEstado=Lc.cEstado
	AND CP.cLocalidad=Lc.cLocalidad
 WHERE EM.nCliente = CASE WHEN @nFolio=0 THEN EM.nCliente ELSE @nFolio END
END 
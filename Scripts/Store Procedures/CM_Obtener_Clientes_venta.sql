sp_eliminastore 'CM_Obtener_Clientes_venta'
GO
CREATE OR ALTER PROCEDURE CM_Obtener_Clientes_venta (
	@cFiltro varchar(50)=''	
)  
AS  
BEGIN  
 -- CM_Obtener_Clientes_venta ''

	SELECT 
	EM.nCliente as codigoCliente,EM.cNombreCompleto as nombreComercial,
	EM.cCalle as calle,Cl.cNombreAsentamiento as colonia,
	LC.cDescripcion as ciudad, Est.cNombreEstado as estado,
	RF.cDescripcion as regimenFiscal,
	CR.cRazonSocial as razonSocial,
	CR.cRFC as rfc,EM.nIDRFC,	
	EM.bActivo as activo,
	EM.cCodigoPostal,
	EM.cTelefono,
	CR.cUso_CFDI
	FROM CAT_Clientes EM (NOLOCK)
	LEFT JOIN CAT_RFC CR (NOLOCK) ON CR.nIDRFC=EM.nIDRFC
	LEFT JOIN CAT_RegimenFiscal RF (NOLOCK) ON RF.nIdRegimenFiscal=CR.cRegimenFiscal
	LEFT JOIN CAT_Colonias CL (NOLOCK) ON CL.cColonia=Em.cColonia
	AND CL.cCodigoPostal=EM.cCodigoPostal
	LEFT JOIN CAT_CodigosPostales CP (NOLOCK) ON CP.cCodigoPostal=EM.cCodigoPostal
	LEFT JOIN CAT_Municipios Mn (NOLOCK) ON CP.cEstado=Mn.cEstado
	AND CP.cMunicipio=Mn.cMunicipio
	LEFT JOIN CAT_Localidades Lc (NOLOCK) ON CP.cEstado=Lc.cEstado
	AND CP.cLocalidad=Lc.cLocalidad
	LEFT JOIN CAT_Estados Est (NOLOCK) ON Est.cEstado=CP.cEstado
	WHERE 1=1		
		AND (@cFiltro IS NULL OR @cFiltro = '')
		OR (
		 (EM.cNombreCompleto LIKE '%' + @cFiltro+ '%') OR (CR.cRFC LIKE '%' + @cFiltro+ '%') )
	ORDER BY EM.dFecha_Registra
END
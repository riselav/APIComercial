CREATE OR ALTER PROCEDURE CAT_Tablero_Clientes (
	@nCliente bigint=0,
	@cRFC varchar(50)='',
	@nRegimen int=0
)  
AS  
BEGIN  
 -- CAT_Tablero_Clientes 0,'',0

 SELECT 
  EM.nCliente as codigoCliente,EM.cNombreCompleto as nombreComercial,
  EM.cCalle as calle,Cl.cNombreAsentamiento as colonia,
  LC.cDescripcion as ciudad, Est.cNombreEstado as estado,
  RF.cDescripcion as regimenFiscal,
  CR.cRazonSocial as razonSocial,
  CR.cRFC as rfc,
  EM.bActivo as activo
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
	 AND ISNULL(RF.nIdRegimenFiscal,0)= CASE WHEN @nRegimen=0 THEN ISNULL(RF.nIdRegimenFiscal,0) ELSE @nRegimen END
	 AND EM.nCliente= CASE WHEN @nCliente=0 THEN EM.nCliente ELSE @nCliente END
	 AND (@cRFC IS NULL OR @cRFC = '' OR CR.cRFC LIKE '%' + @cRFC+ '%')
END
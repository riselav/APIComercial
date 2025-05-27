SP_ELIMINASTORE 'CAT_CON_CAT_Empleados'
GO

-- Exec CAT_CON_CAT_Empleados 0
CREATE PROCEDURE CAT_CON_CAT_Empleados (@nFolio int )  
AS  
BEGIN  
 -- RST_CON_CAT_Empleados 1

 SELECT EM.nEmpleado,EM.cNombre,EM.cApellidoPaterno,EM.cApellidoMaterno,EM.nEmpresa,EM.dFechaNacimiento,EM.dFechaIngreso,EM.dFechaBaja,
 EM.cRFC,EM.cCURP,EM.cColonia,EM.cCodigoPostal,EM.cDomicilio,EM.cReferencia,EM.nSucursal,EM.nPuesto,EM.nDepartamento,EM.bActivo,
 CP.cEstado, Es.cNombreEstado,  CP.cMunicipio,Mn.cNombreMunicipio,CP.cLocalidad,LC.cDescripcion as cNombreLocalidad, CL.cNombreAsentamiento,
 P.cDescripcion as cNombrePuesto

 FROM CAT_Empleados EM (NOLOCK)
 JOIN CAT_Colonias CL (NOLOCK) ON CL.cColonia=Em.cColonia
	AND CL.cCodigoPostal=EM.cCodigoPostal
 JOIN CAT_CodigosPostales CP (NOLOCK) ON CP.cCodigoPostal=EM.cCodigoPostal
 JOIN CAT_Municipios Mn (NOLOCK) ON CP.cEstado=Mn.cEstado
	AND CP.cMunicipio=Mn.cMunicipio
	Join CAT_Estados Es (Nolock) on Es.cEstado = MN.cEstado 
 JOIN CAT_Localidades Lc (NOLOCK) ON CP.cEstado=Lc.cEstado
	AND CP.cLocalidad=Lc.cLocalidad
	Join CAT_Puestos as P (Nolock) on P.nPuesto = EM.nPuesto 
 WHERE (EM.nEmpleado = @nFolio  or @nFolio =0)
END 

CREATE OR ALTER PROC CAT_CON_DireccionPorCodigoPostal(
	@cCodigoPostal varchar(20)
)
AS

-- EXEC CAT_CON_DireccionPorCodigoPostal 80050

DECLARE @CAT_Paises as table(Id varchar(3),Nombre varchar(100))

INSERT INTO @CAT_Paises
SELECT 'MEX','MÉXICO'

SELECT P.Id as cPais,P.Nombre as cNombrePais,E.cEstado,E.cNombreEstado,
M.cMunicipio,M.cNombreMunicipio,L.cLocalidad as cCiudad,L.cDescripcion as cNombreCiudad
FROM CAT_Estados E (NOLOCK)
JOIN @CAT_Paises P ON P.id=E.cPais
JOIN CAT_Municipios M (NOLOCK) ON M.cEstado=E.cEstado
JOIN CAT_Localidades L (NOLOCK) ON L.cEstado=E.cEstado 
JOIN CAT_CodigosPostales CP (NOLOCK) ON CP.cEstado=L.cEstado
	AND CP.cLocalidad=L.cLocalidad
	AND CP.cMunicipio=M.cMunicipio
WHERE CP.cCodigoPostal=@cCodigoPostal


SELECT Col.cColonia,Col.cNombreAsentamiento as cNombreColonia
FROM CAT_Colonias Col (NOLOCK) 
JOIN CAT_CodigosPostales CP (NOLOCK) ON Col.cCodigoPostal=CP.cCodigoPostal
WHERE CP.cCodigoPostal=@cCodigoPostal

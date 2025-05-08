SP_ELIMINASTORE 'CAT_IME_CAT_Empleados'
GO

--SELECT * FROM Empleados (NOLOCK)

CREATE PROCEDURE CAT_IME_CAT_Empleados (        
	@nFolio int,  
	@cNombre Varchar(50),
	@cApellidoPaterno Varchar(50),
	@cApellidoMaterno Varchar(50)=NULL,
	@nEmpresa as int,
	@dFechaNacimiento date,
	@dFechaIngreso date,
	@dFechaBaja date=NULL,
	@cRFC Varchar(20)=NULL,
	@cCURP Varchar(50)=NULL,
	@cCveColonia Varchar(50),
	@cCodigoPostal Varchar(50),
	@cDomicilio Varchar(50),
	@cReferencia Varchar(100)=NULL,
	@nSucursal int,
	@nPuesto int=NULL,
	@nDepartamento int=NULL,
	@bActivo as bit,        
	@cUsuario as Varchar(50),        
	@cNombreMaquina as Varchar(50)        
)        
AS         
BEGIN        

--=================================================================================================================        
-- Si el Folio es cero, indica que es un nuevo registro,        
--=================================================================================================================        
IF @nFolio =0         
BEGIN        
  DECLARE @nFolioSig AS INT =  (SELECT ISNULL(MAX(nEmpleado),0) FROM CAT_Empleados (NOLOCK)) + 1        
        
  INSERT INTO CAT_Empleados (nEmpleado,cNombre,cApellidoPaterno,cApellidoMaterno,nEmpresa,dFechaNacimiento,dFechaIngreso,dFechaBaja,
  cRFC,cCURP,cColonia,cCodigoPostal,cDomicilio,cReferencia,nSucursal,nPuesto,nDepartamento,
  bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)        
  Select @nFolioSig, @cNombre,@cApellidoPaterno,@cApellidoMaterno,@nEmpresa,@dFechaNacimiento,@dFechaIngreso,@dFechaBaja,
  @cRFC,@cCURP,@cCveColonia,@cCodigoPostal,@cDomicilio,@cReferencia,@nSucursal,@nPuesto,@nDepartamento,
  @bActivo, @cUsuario, @cNombreMaquina, getdate()
        
  RETURN @nFolioSig        
END         
ELSE        
BEGIN   
  --=================================================================================================================        
  -- Si el Folio es mayor a cero, se valora el campo bActivo:        
  --    Si el valor es 1, entonces indica que es una modificación        
  --    Si el valor es 0, entonces se trata de una cancelacion        
  --=================================================================================================================        
  
  -- Actualiza el registro indicado        
  IF @bActivo =1
  BEGIN          
    UPDATE R SET cNombre= @cNombre,    
				 cApellidoPaterno = @cApellidoPaterno,      
				 cApellidoMaterno = @cApellidoMaterno,    
				 nEmpresa=@nEmpresa,
			     dFechaNacimiento= @dFechaNacimiento, 
				 dFechaIngreso=@dFechaIngreso,
				 dFechaBaja=@dFechaBaja,
				 cRFC= @cRFC,
			     cCURP=@cCURP,
			     cColonia=@cCveColonia,
			     cCodigoPostal=@cCodigoPostal,
			     cDomicilio=@cDomicilio,
			     cReferencia=@cReferencia,
			     nSucursal=@nSucursal,
				 nPuesto=@nPuesto,
				 nDepartamento=@nDepartamento,
			     bActivo= @bActivo,        
				 cUsuario_Modifica= @cUsuario,        
				 cMaquina_Modifica= @cNombreMaquina,        
				 dFecha_Modifica = getdate ()        
    FROM CAT_Empleados as R        
    WHERE nEmpleado = @nFolio
        
    RETURN @nFolio        
  END         
        
  -- Cancela el registro indicado        
         
    UPDATE R SET bActivo= @bActivo,    
		dFechaBaja=@dFechaBaja,
        cRegistro_Cancela=  @cUsuario,        
        cMaquina_Cancela=  @cNombreMaquina,        
        dFecha_Cancela = GETDATE ()        
    FROM CAT_Empleados as R        
    WHERE nEmpleado = @nFolio        
        
    RETURN @nFolio        
  END        
END
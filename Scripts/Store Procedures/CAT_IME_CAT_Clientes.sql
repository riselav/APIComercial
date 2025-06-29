SP_ELIMINASTORE 'CAT_IME_Clientes'
GO

--SELECT * FROM CAT_Clientes (NOLOCK)  
  
CREATE PROCEDURE CAT_IME_Clientes (          
	@nFolio int,    
	@cRazonSocial Varchar(500),
	@cNombreCompleto Varchar(150),
	@nTipoPersona tinyint,
	@cRFC varchar(50),
	@cRegimenFiscal varchar(5),
	@cCalle Varchar(200),  
	@cNumExt Varchar(20)=NULL,  
	@cNumInt Varchar(20)=NULL, 
	@cCodigoPostal Varchar(20),
	@cCveColonia Varchar(50),  
	@cTelefono Varchar(30)=NULL,  
	@cSeniasParticulares Varchar(500)=NULL,  
	@bActivo as bit,          
	@cUsuario as Varchar(50),          
	@cNombreMaquina as Varchar(50),  
	@nSucursalRegistro int=NULL  
)          
AS           
BEGIN          
  
--=================================================================================================================          
-- Si el Folio es cero, indica que es un nuevo registro,          
--=================================================================================================================  
  
IF @nSucursalRegistro=0 SET @nSucursalRegistro=NULL  
  
DECLARE @nFolioSig AS INT   
  
IF @nFolio =0           
BEGIN          
  SET @nFolioSig =  (SELECT ISNULL(MAX(CONVERT(int,RIGHT(nCliente,6))),0) FROM CAT_Clientes (NOLOCK)) + 1        
    
  DECLARE @nID bigint='1'+RIGHT('000'+CONVERT(varchar(3),@nSucursalRegistro),3)+ RIGHT('000000'+CONVERT(varchar(6),@nFolioSig),6)  
  
  INSERT INTO CAT_Clientes (nCliente,cNombreCompleto,cColonia,cCodigoPostal,cCalle,cNumExt,cNumInt,  
  nTipoPersona,cTelefono,cSeniasParticulares,nSucursalRegistro,
  bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)          
  SELECT @nID, @cNombreCompleto,@cCveColonia,@cCodigoPostal,@cCalle,@cNumExt,@cNumInt,  
  @nTipoPersona,@cTelefono,@cSeniasParticulares,@nSucursalRegistro,  
  @bActivo, @cUsuario, @cNombreMaquina, getdate()  
          
  DECLARE @nIDRFC bigint=(SELECT ISNULL(MAX(nIDRFC),0) FROM CAT_RFC (NOLOCK)) + 1
  DECLARE @cDomicilio varchar(500)=(
	SELECT @cCalle + ' ' + @cNumExt + ' ' + @cNumInt + ' ' + Cl.cNombreAsentamiento 
	--ANTONIO ROSALES S/N COL. NAVOLATO CENTRO
	FROM CAT_Colonias Cl (NOLOCK)
	JOIN CAT_CodigosPostales CP(NOLOCK)ON CP.cCodigoPostal=CL.cCodigoPostal
	WHERE Cl.cCodigoPostal=@cCodigoPostal AND CL.cColonia=@cCveColonia
  )
  
  INSERT INTO CAT_RFC(nIDRFC,cRazonSocial,cRFC,cCP,cDomicilio,cUso_CFDI,cRegimenFiscal,bActivo)
  SELECT @nIDRFC,@cRazonSocial,@cRFC,@cCodigoPostal,@cDomicilio,'' as cUso_CFDI,@cRegimenFiscal,1

  UPDATE CAT_Clientes SET nIDRFC=@nIDRFC WHERE nCliente=@nID

  RETURN @nFolioSig  
END           
ELSE          
BEGIN     
  --=================================================================================================================          
  -- Si el Folio es mayor a cero, se valora el campo bActivo:          
  --    Si el valor es 1, entonces indica que es una modificación          
  --    Si el valor es 0, entonces se trata de una cancelacion          
  --=================================================================================================================          
  ----SET @nFolioSig= CONVERT(int,RIGHT( CONVERT(varchar(6), ltrim(@nFolio)),6))    
  -- toma los ultimos 6 digitos del ID y lo devuelve  
    SET @nFolioSig= Cast(RIGHT(Cast(@nFolio as varchar(10)),6) as int)  
  
  -- Actualiza el registro indicado          
  IF @bActivo =1  
  BEGIN            
    UPDATE R SET cNombreCompleto= @cNombreCompleto,      
     cColonia=@cCveColonia,  
     cCodigoPostal=@cCodigoPostal,  
     cCalle=@cCalle,  
     cNumExt=@cNumExt,  
     cNumInt=@cNumInt,  
     cTelefono=@cTelefono,  
     cSeniasParticulares=@cSeniasParticulares,
	 nTipoPersona=@nTipoPersona,
     bActivo= @bActivo,          
     cUsuario_Modifica= @cUsuario,          
     cMaquina_Modifica= @cNombreMaquina,          
     dFecha_Modifica = GETDATE ()          
    FROM CAT_Clientes as R          
    WHERE nCliente = @nFolio  
          
	UPDATE R SET cRazonSocial=@cRazonSocial,cRFC=@cRFC,
				 cRegimenFiscal=@cRegimenFiscal
	FROM CAT_RFC R (NOLOCK)
	JOIN CAT_Clientes C (NOLOCK) ON R.nIDRFC=C.nIDRFC
	WHERE C.nCliente=@nFolio

    RETURN @nFolioSig          
  END           
          
  -- Cancela el registro indicado          
           
    UPDATE R SET bActivo= @bActivo,   
  cUsuario_Cancela=  @cUsuario,          
        cMaquina_Cancela=  @cNombreMaquina,          
        dFecha_Cancela = GETDATE ()          
    FROM CAT_Clientes as R          
    WHERE nCliente = @nFolio  
   
    RETURN @nFolioSig          
  END          
END

SP_ELIMINASTORE 'CM_IME_CAJ_RegistroApertura'
GO
--SELECT * FROM Empleados (NOLOCK)

CREATE PROCEDURE CM_IME_CAJ_RegistroApertura (        
	@nFolio bigint output,  
	@nSucursal int,
	@nCaja int,
	@nTurno int,
	@dFecha as date,
	@nDotacionInicial decimal(18,4),
	@nEmpleado int,
	@nUsuarioAutoriza int=NULL,
	@nEstatus int,
	@bActivo as bit,        
	@cUsuario as Varchar(50),        
	@cNombreMaquina as Varchar(50)        
)        
AS         
BEGIN
	DECLARE @nFolioSig AS INT =  isnull ((SELECT MAX(CONVERT(int,RIGHT(nIDApertura,8))) FROM CAJ_RegistrosAperturaCaja (NOLOCK)),0) + 1    
	
	If ISNULL(@nFolio,0)=0
		SET @nFolio='1' + RIGHT('00000'+CONVERT(varchar(5),@nSucursal),5) + RIGHT('00000000'+ CONVERT(varchar(8),@nFolioSig),8)
        
	INSERT INTO CAJ_RegistrosAperturaCaja (nIDApertura,nIDSucursal,nIDCaja,nIDTurno,dFecha,nDotacionInicial,
	nIDEmpleado,nIDUsuarioAutoriza,nEstatus,
	bActivo, cUsuario_Registra, cMaquina_Registra,dFecha_Registra)        
	SELECT @nFolio, @nSucursal,@nCaja,@nTurno,@dFecha,@nDotacionInicial,
	@nEmpleado,@nUsuarioAutoriza,1,
	@bActivo, @cUsuario, @cNombreMaquina, getdate()
        
	RETURN @nFolioSig
END
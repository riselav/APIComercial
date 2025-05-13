SP_ELIMINASTORE 'CM_IME_CAJ_MovimientoCaja'
GO
--SELECT * FROM Empleados (NOLOCK)  
  
CREATE PROCEDURE CM_IME_CAJ_MovimientoCaja (          
	@nFolio bigint output,    
	@nTipoRegistro int,  
	@nSucursal int,   
	@nIDApertura bigint,  
	@dFecha date,  
	@nConceptoCaja int=NULL,   
	@nEmpleado int,  
	@nImporte decimal(18,4),  
	@cObservaciones varchar(5000)=NULL,  
	@bActivo as bit,    
	@cUsuario as Varchar(50),          
	@cNombreMaquina as Varchar(50),  
	@cUsuarioAutoriza varchar(50)='',  
	@dFechaRegistro datetime=NULL,  
	@bRegistroEspecial bit=0  
)          
AS           
BEGIN  
 DECLARE @nConsecutivo int  
  
 --DECLARE @nFolioSig AS INT =  ((SELECT MAX(CONVERT(int,RIGHT(nIDRegistroCaja,8))) FROM CAJ_MovimientosCaja (NOLOCK) WHERE nSucursal=@nSucursal ) + 1)  
 DECLARE @nFolioSig AS INT =  ISNULL((SELECT MAX(CONVERT(int,RIGHT(nIDRegistroCaja,8))) FROM CAJ_MovimientosCaja (NOLOCK) WHERE nIDApertura=@nIDApertura),0)+1  
 DECLARE @nIDCaja int=(SELECT nIDCaja FROM CAJ_RegistrosAperturaCaja (NOLOCK) WHERE nIDApertura= @nIDApertura) 
 DECLARE @nConsecutivoSig AS INT =  ISNULL((SELECT MAX(CONVERT(int,RIGHT(nIDRegistroCaja,8)))   
									 FROM CAJ_MovimientosCaja (NOLOCK)   
									 WHERE nSucursal=@nSucursal 
										AND nTipoRegistroCaja=@nTipoRegistro AND nIDApertura=@nIDApertura),0) + 1  
   
 IF @dFechaRegistro IS NULL SET @dFechaRegistro=(SELECT GETDATE())  
  
 If ISNULL(@nFolio,0)=0  
	SET @nFolio='1' + RIGHT('000'+CONVERT(varchar(3),@nSucursal),3)+ RIGHT('0000'+CONVERT(varchar(4),@nIDCaja),4) + RIGHT('000000'+ CONVERT(varchar(6),@nFolioSig),6)   
  
  DECLARE @nEfecto int=(SELECT nEfecto FROM CAT_TiposRegistroCaja(NOLOCK) WHERE nTipoRegistroCaja=@nTipoRegistro)  
 --DECLARE @dFecha as date=(SELECT dFecha FROM CAJ_RegistrosAperturaCaja (NOLOCK) WHERE nIDApertura=@nIDApertura)  
  
 INSERT INTO CAJ_MovimientosCaja (nIDRegistroCaja, nTipoRegistroCaja,nSucursal, nIDApertura, nConceptoCaja ,   
 nConsecutivo,nEmpleadoInvolucrado,nImporte, nEfecto,dFecha,cObservaciones,bRegistroEspecial,  
 bActivo, cUsuario_Registra, cMaquina_Registra,dFecha_Registra,cUsuarioAutoriza_Registro)  
  
 SELECT @nFolio, @nTipoRegistro, @nSucursal, @nIDApertura,@nConceptoCaja,  
 @nConsecutivoSig,@nEmpleado,@nImporte,@nEfecto,@dFecha,@cObservaciones,@bRegistroEspecial,  
 @bActivo, @cUsuario, @cNombreMaquina, @dFechaRegistro,@cUsuarioAutoriza  
          
 RETURN @nConsecutivoSig          
END
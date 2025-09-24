--RST_IME_CAJ_CorteCaja
SP_ELIMINASTORE 'RST_IME_CAJ_CorteCaja'
GO
CREATE PROCEDURE [dbo].[RST_IME_CAJ_CorteCaja] (        
	@nFolio bigint,  
	@nSucursal int,
	@nCaja int,
	@dFechaCorte datetime=NULL, 
	@nUsuarioAutoriza int=NULL,
	@bActivo as bit,        
	@cUsuario as Varchar(50),        
	@cNombreMaquina as Varchar(50),
	@nConsecutivo bigint output
)        
AS         
BEGIN
	BEGIN TRY
		/*	
		@nTurno int,
		@dFecha as date,
		@nDotacionInicial decimal(18,4),
		*/

		IF @dFechaCorte IS NULL SET @dFechaCorte=(SELECT GETDATE())

		DECLARE @nIDApertura bigint=(SELECT nIDApertura FROM CAJ_RegistrosAperturaCaja (NOLOCK) WHERE nIDSucursal=@nSucursal AND nIDCaja=@nCaja AND nEstatus=1) -- Abierta 

		IF @nIDApertura IS NULL RETURN -1 -- Debe existir apertura para la sucursal y caja indicada para realizar el corte

		DECLARE @nDotacionInicial decimal(18,4)

		SELECT @nDotacionInicial=SUM(nImporte)
			   FROM CAJ_MovimientosCaja (NOLOCK)
			   WHERE nIDApertura=@nIDApertura AND bActivo=1 AND nIDCorteCaja IS NULL
			   AND nTipoRegistroCaja=1

		DECLARE @nTotalRetiros decimal(18,4),@nTotalGastos decimal(18,4),@nTotalIngresos decimal(18,4)

		SELECT @nTotalRetiros=SUM(nImporte)
			   FROM CAJ_MovimientosCaja (NOLOCK)
			   WHERE nIDApertura=@nIDApertura AND bActivo=1 AND nIDCorteCaja IS NULL
			   AND nTipoRegistroCaja=2

		IF @nTotalRetiros IS NULL SET @nTotalRetiros=0

		SELECT @nTotalIngresos=SUM(nImporte)
			   FROM CAJ_MovimientosCaja (NOLOCK)
			   WHERE nIDApertura=@nIDApertura AND bActivo=1 AND nIDCorteCaja IS NULL
			   AND nTipoRegistroCaja=3

		IF @nTotalIngresos IS NULL SET @nTotalIngresos=0

		SELECT @nTotalGastos=SUM(nImporte)
			   FROM CAJ_MovimientosCaja (NOLOCK)
			   WHERE nIDApertura=@nIDApertura AND bActivo=1 AND nIDCorteCaja IS NULL
			   AND nTipoRegistroCaja=4

		IF @nTotalGastos IS NULL SET @nTotalGastos=0

		

		IF ISNULL(@nFolio,0)=0
		BEGIN
			SET @nConsecutivo= ISNULL((SELECT MAX(CONVERT(int,RIGHT(nIDCorteCaja,8))) FROM CAJ_CortesCaja (NOLOCK) WHERE nSucursal=@nSucursal),0)+1        
         
			SET @nFolio='1' + RIGHT('00000'+CONVERT(varchar(5),@nSucursal),5) + RIGHT('00000000'+ CONVERT(varchar(8),@nConsecutivo),8)   
        
			INSERT INTO CAJ_CortesCaja (nIDCorteCaja,nSucursal,nIDApertura,dFechaCorte,
			nDotacionInicial,nTotalRetiros,nTotalGastos,nTotalIngresos,
			bActivo, bImpreso,cUsuario_Registra, cMaquina_Registra,dFecha_Registra)        
			SELECT @nFolio, @nSucursal,@nIDApertura,@dFechaCorte,
			@nDotacionInicial,@nTotalRetiros,@nTotalGastos,@nTotalIngresos,
			@bActivo, 0 as bImpreso, @cUsuario, @cNombreMaquina, getdate()
			
		END
		ELSE
		BEGIN
			SET @nConsecutivo= (SELECT CONVERT(int,RIGHT(@nFolio,8)))
		END
        
		RETURN @nConsecutivo
	END TRY
	BEGIN CATCH
		/*
		SELECT -1,       
		ERROR_PROCEDURE()+':'+      
        ERROR_MESSAGE() as cError,      
      
        ERROR_NUMBER() AS ErrorNumber,      
        ERROR_SEVERITY() AS ErrorSeverity,      
        ERROR_STATE() as ErrorState,      
        ERROR_PROCEDURE() as ErrorProcedure,      
        ERROR_LINE() as ErrorLine,      
        ERROR_MESSAGE() as ErrorMessage;  
		*/
		SELECT -1
	END CATCH
END
GO



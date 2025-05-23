--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_IME_CatImpuestos'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_IME_CatImpuestos;
END;
GO


CREATE procedure [dbo].[CAT_IME_CatImpuestos] (  
@nImpuesto as int,   
@cDescripcion Varchar(200),
@nPorcentaje decimal(18,2),
@nTipo tinyint,
@bImpuestoImporte as bit,
@bExcento as bit,
@cImpuesto Varchar(8),
@cTipoFactor Varchar(10),
@nTasaOCuota decimal(18,6),
@bActivo as bit,  
@cUsuario as Varchar(50),  
@cNombreMaquina as Varchar(50)  
)  
As   
Begin  

--=================================================================================================================  
-- Si el Folio es cero, indica que es un nuevo registro,  
--=================================================================================================================  
if @nImpuesto =0   
Begin  
 Declare @nFolioSig as int =  (Select Isnull(max(nImpuesto),0)  From CAT_Impuestos) + 1  
  
 Insert into CAT_Impuestos (nImpuesto, cDescripcion, nPorcentaje, nTipo,bImpuestoImporte,bExcento,c_Impuesto,cTipoFactor,nTasaOCuota, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)  
 Select @nFolioSig,@cDescripcion, @nPorcentaje, @nTipo,@bImpuestoImporte,@bExcento,@cImpuesto,@cTipoFactor,@nTasaOCuota, 1, @cUsuario, @cNombreMaquina, getdate()   
  
 return @nFolioSig  
  
End   
Else  
Begin  
  
--=================================================================================================================  
-- Si el Folio es mayor a cero, se valora el campo bActivo:  
--    Si el valor es 1, entonces indica que es una modificación  
--    Si el valor es 0, entonces se trata de una cancelacion  
--=================================================================================================================  
-- Actualiza el registro indicado  
  
 if @bActivo =1    
 Begin  
    
  Update E Set cDescripcion= @cDescripcion,  
		 nPorcentaje=@nPorcentaje, 
		 nTipo=@nTipo,
		 bImpuestoImporte=@bImpuestoImporte,
		 bExcento=@bExcento,
		 c_Impuesto=@cImpuesto,
		 cTipoFactor=@cTipoFactor,
		 nTasaOCuota=@nTasaOCuota,
		 bActivo= @bActivo,  
         cUsuario_Modifica= @cUsuario,  
         cMaquina_Modifica= @cNombreMaquina,  
         dFecha_Modifica = getdate ()  
  From CAT_Impuestos as E  
  Where nImpuesto = @nImpuesto
  
  Return @nImpuesto  
 End   
  
-- Cancela el registro indicado  
   
 Update R Set  bActivo= @bActivo,   
      cUsuario_Cancela= @cUsuario,  
      cMaquina_Cancela= @cNombreMaquina,  
      dFecha_Cancela = getdate ()  
  From CAT_Impuestos as R  
  Where nImpuesto = @nImpuesto
  
  Return @nImpuesto
End  
  
End   
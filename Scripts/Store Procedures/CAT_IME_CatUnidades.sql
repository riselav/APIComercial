--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_IME_CatUnidades'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_IME_CatUnidades;
END;
GO


CREATE procedure [dbo].[CAT_IME_CatUnidades] (  
@nUnidad as int,  
@cDescripcion Varchar(200),
@nTipoUnidad smallint,
@cClaveSAT as varchar(10),
@cAbreviatura as Varchar(50),  
@bActivo as bit,  
@cUsuario as Varchar(50),  
@cNombreMaquina as Varchar(50)  
)  
As   
Begin  

--=================================================================================================================  
-- Si el Folio es cero, indica que es un nuevo registro,  
--=================================================================================================================  
if @nUnidad =0   
Begin  
 Declare @nFolioSig as int =  (Select Isnull(max(nUnidad),0)  From CAT_Unidades) + 1  
  
 Insert into CAT_Unidades (nUnidad, cDescripcion,nTipoUnidad,cClaveSAT,cAbreviatura, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)  
 Select @nFolioSig,@cDescripcion,@nTipoUnidad,@cClaveSAT,@cAbreviatura, 1, @cUsuario, @cNombreMaquina, getdate()   
  
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
  nTipoUnidad=@nTipoUnidad,
		cClaveSAT=@cClaveSAT,
		cAbreviatura=@cAbreviatura,
		 bActivo= @bActivo,  
         cUsuario_Modifica= @cUsuario,  
         cMaquina_Modifica= @cNombreMaquina,  
         dFecha_Modifica = getdate ()  
  From CAT_Unidades as E  
  Where nUnidad = @nUnidad
  
  Return @nUnidad  
 End   
  
-- Cancela el registro indicado  
   
 Update R Set  bActivo= @bActivo,   
      cUsuario_Cancela= @cUsuario,  
      cMaquina_Cancela= @cNombreMaquina,  
      dFecha_Cancela = getdate ()  
  From CAT_Unidades as R  
  Where nUnidad = @nUnidad
  
  Return @nUnidad
End  
  
End   
--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_IME_CatProductosBase'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_IME_CatProductosBase;
END;
GO


CREATE procedure [dbo].[CAT_IME_CatProductosBase] (  
@nProductoBase as int,   
@cDescripcion Varchar(200),
@nTipoUnidad tinyint,
@bActivo as bit,  
@cUsuario as Varchar(50),  
@cNombreMaquina as Varchar(50)  
)  
As   
Begin  

--=================================================================================================================  
-- Si el Folio es cero, indica que es un nuevo registro,  
--=================================================================================================================  
if @nProductoBase =0   
Begin  
 Declare @nFolioSig as int =  (Select Isnull(max(nProductoBase),0)  From CAT_ProductosBase) + 1  
  
 Insert into CAT_ProductosBase (nProductoBase, cDescripcion, nTipoUnidad, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)  
 Select @nFolioSig,@cDescripcion, @nTipoUnidad, 1, @cUsuario, @cNombreMaquina, getdate()   
  
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
		 bActivo= @bActivo,  
         cUsuario_Modifica= @cUsuario,  
         cMaquina_Modifica= @cNombreMaquina,  
         dFecha_Modifica = getdate ()  
  From CAT_ProductosBase as E  
  Where nProductoBase = @nProductoBase
  
  Return @nProductoBase  
 End   
  
-- Cancela el registro indicado  
   
 Update R Set  bActivo= @bActivo,   
      cUsuario_Cancela= @cUsuario,  
      cMaquina_Cancela= @cNombreMaquina,  
      dFecha_Cancela = getdate ()  
  From CAT_ProductosBase as R  
  Where nProductoBase = @nProductoBase
  
  Return @nProductoBase
End  
  
End   
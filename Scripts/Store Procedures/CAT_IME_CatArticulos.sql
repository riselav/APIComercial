--USE [Voalaft_Navola]
--GO

IF EXISTS (
    SELECT * 
    FROM INFORMATION_SCHEMA.ROUTINES
    WHERE ROUTINE_NAME = 'CAT_IME_CatArticulos'
    AND ROUTINE_TYPE = 'PROCEDURE'
)
BEGIN
    -- Si existe, elimínalo
    DROP PROCEDURE CAT_IME_CatArticulos;
END;
GO


CREATE procedure [dbo].[CAT_IME_CatArticulos] (  
@nArticulo as int,  
@cClave Varchar(20),
@cDescripcion Varchar(200),
@nUnidad smallint,
@cPresentacion as varchar(50),
@nLinea smallint,
@nSublinea smallint,
@nMarca smallint,
@cClaveSAT as Varchar(50),  
@bLote as bit,  
@bSerie as bit,  
@bPedimento as bit,  
@bInsumoFinal as bit,  
@bProductoBase as bit,  
@nIdProductoBase int,
@nIdUnidadRelacional  int,
@nEquivalencia decimal(18,6),
@bManejaInventario as bit,  
@bActivo as bit,  
@cUsuario as Varchar(50),  
@cNombreMaquina as Varchar(50)  
)  
As   
Begin  

--=================================================================================================================  
-- Si el Folio es cero, indica que es un nuevo registro,  
--=================================================================================================================  
if @nArticulo =0   
Begin  
 Declare @nFolioSig as int =  (Select Isnull(max(nIdArticulo),0)  From CAT_Articulos) + 1  
  
 Insert into CAT_Articulos (nIDArticulo,cClave, cDescripcion,nUnidad,cPresentacion,nLinea,nSublinea, nMarca,cClaveSAT,bLote,bSerie,bPedimento,bInsumoFinal
	,bProductoBase,nIdProductoBase,nIdUnidadRelacional,nEquivalencia,bManejaInventario, bActivo, cUsuario_Registra, cMaquina_Registra, dFecha_Registra)  
 Select @nFolioSig,@cClave, @cDescripcion,@nUnidad,@cPresentacion,@nLinea,@nSublinea, @nMarca,@cClaveSAT,@bLote,@bSerie,@bPedimento,@bInsumoFinal
	,@bProductoBase,@nIdProductoBase,@nIdUnidadRelacional,@nEquivalencia,@bManejaInventario, 1, @cUsuario, @cNombreMaquina, getdate()   
  
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
  cClave=@cClave, 
  nUnidad=@nUnidad,
  cPresentacion=@cPresentacion,
  nLinea=@nLinea,
  nSublinea=@nSublinea, 
  nMarca=@nMarca,
  cClaveSAT=@cClaveSAT,
  bLote=@bLote,
  bSerie=@bSerie,
  bPedimento=@bPedimento,
  bInsumoFinal=@bInsumoFinal,
  bProductoBase=@bProductoBase,
  nIdProductoBase=@nIdProductoBase,
  nIdUnidadRelacional=@nIdUnidadRelacional,
  nEquivalencia=@nEquivalencia,
  bManejaInventario=@bManejaInventario,
		 bActivo= @bActivo,  
         cUsuario_Modifica= @cUsuario,  
         cMaquina_Modifica= @cNombreMaquina,  
         dFecha_Modifica = getdate ()  
  From CAT_Articulos as E  
  Where nIdArticulo = @nArticulo
  
  Return @nArticulo  
 End   
  
-- Cancela el registro indicado  
   
 Update R Set  bActivo= @bActivo,   
      cUsuario_Cancela= @cUsuario,  
      cMaquina_Cancela= @cNombreMaquina,  
      dFecha_Cancela = getdate ()  
  From CAT_Articulos as R  
  Where nIdArticulo = @nArticulo
  
  Return @nArticulo
End  
  
End   
sp_eliminaStore 'RST_IME_CAT_EncabezadosSucursal'

GO

--select * from CAT_EncabezadosSucursal
CREATE PROCEDURE RST_IME_CAT_EncabezadosSucursal (        
@nSucursal as int,        
@cEncabezado1 Varchar(50),  
@cEncabezado2 Varchar(50), 
@cEncabezado3 Varchar(50), 
@cEncabezado4 Varchar(50), 
@cEncabezado5 Varchar(50), 
@cEncabezado6 Varchar(50), 
@cEncabezado7 Varchar(50), 
@cEncabezado8 Varchar(50), 
@cEncabezado9 Varchar(50)     
)        
As         
Begin        
        
--=================================================================================================================        
-- Si el Folio es cero, indica que es un nuevo registro,        
--=================================================================================================================        
if not exists (select top 1 * from CAT_EncabezadosSucursal where nSucursal = @nSucursal)     
	Begin        	    
        
	 Insert into CAT_EncabezadosSucursal (nSucursal,cEncabezado1,cEncabezado2, cEncabezado3, cEncabezado4, cEncabezado5, cEncabezado6,cEncabezado7,cEncabezado8,cEncabezado9)        
	 Select @nSucursal, @cEncabezado1,@cEncabezado2, @cEncabezado3, @cEncabezado4, @cEncabezado5, @cEncabezado6,@cEncabezado7,@cEncabezado8,@cEncabezado9
	 
	 return @nSucursal        
        
	End         
      
Begin        
        
 
-- Actualiza el registro indicado        
        
 if exists (select top 1 * from CAT_EncabezadosSucursal where nSucursal = @nSucursal)             
 Begin                  
	  Update R Set cEncabezado1 = @cEncabezado1,
				  cEncabezado2 = @cEncabezado2, 
				  cEncabezado3 = @cEncabezado3, 
				  cEncabezado4 = @cEncabezado4, 
				  cEncabezado5 = @cEncabezado5, 
				  cEncabezado6 = @cEncabezado6,
				  cEncabezado7 = @cEncabezado7,
				  cEncabezado8 = @cEncabezado8,
				  cEncabezado9 = @cEncabezado9
	  From CAT_EncabezadosSucursal as R        
	  Where nSucursal = @nSucursal        
        
	  Return @nSucursal        
 End         
        
End        
        
End 
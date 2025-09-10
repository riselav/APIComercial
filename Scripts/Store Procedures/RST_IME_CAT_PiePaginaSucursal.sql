sp_eliminaStore 'RST_IME_CAT_PiePaginaSucursal'

GO

--select * from CAT_EncabezadosSucursal
CREATE PROCEDURE RST_IME_CAT_PiePaginaSucursal (        
@nSucursal as int,        
@cPiePagina1 Varchar(50),  
@cPiePagina2 Varchar(50), 
@cPiePagina3 Varchar(50), 
@cPiePagina4 Varchar(50), 
@cPiePagina5 Varchar(50), 
@cPiePagina6 Varchar(50), 
@cPiePagina7 Varchar(50), 
@cPiePagina8 Varchar(50), 
@cPiePagina9 Varchar(50)     
)        
As         
Begin        
        
--=================================================================================================================        
-- Si el Folio es cero, indica que es un nuevo registro,        
--=================================================================================================================        
if not exists (select top 1 * from CAT_PieTicketSucursal where nSucursal = @nSucursal)     
	Begin        	    
        
	 Insert into CAT_PieTicketSucursal (nSucursal,cPieTicket1,cPieTicket2, cPieTicket3, cPieTicket4, cPieTicket5, cPieTicket6,cPieTicket7,cPieTicket8,cPieTicket9)        
	 Select @nSucursal, @cPiePagina1,@cPiePagina2, @cPiePagina3, @cPiePagina4, @cPiePagina5, @cPiePagina6,@cPiePagina7,@cPiePagina8,@cPiePagina9
	 
	 return @nSucursal        
        
	End         
      
Begin        
        
 
-- Actualiza el registro indicado        
        
 if exists (select top 1 * from CAT_PieTicketSucursal where nSucursal = @nSucursal)             
 Begin                  
	  Update R Set cPieTicket1 = @cPiePagina1,
				  cPieTicket2 = @cPiePagina2, 
				  cPieTicket3 = @cPiePagina3, 
				  cPieTicket4 = @cPiePagina4, 
				  cPieTicket5 = @cPiePagina5, 
				  cPieTicket6 = @cPiePagina6,
				  cPieTicket7 = @cPiePagina7,
				  cPieTicket8 = @cPiePagina8,
				  cPieTicket9 = @cPiePagina9
	  From CAT_PieTicketSucursal as R        
	  Where nSucursal = @nSucursal        
        
	  Return @nSucursal        
 End         
        
End        
        
End 
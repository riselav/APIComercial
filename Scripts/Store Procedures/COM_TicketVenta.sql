

sp_eliminastore 'COM_TicketVenta'

GO


-- Exec COM_TicketVenta 1, 100100100000065
Create procedure COM_TicketVenta (@nSucursal int, @nFolio bigint)
As 
Begin 


Set nocount on

Declare @TotalCaracteres int =40
Declare @LongImporte int =9
Declare @LongCantidad int =4
Declare @LongDescripcion int = (@TotalCaracteres - @LongImporte - @LongCantidad )
Declare @bImprimeEncabezadoyPie as bit =Isnull((SELECT isnull(cValor,0) FROM CAT_Parametros (NOLOCK) WHERE cParametro='RST_ImprimeCabeceroyTicketEnCuenta'),0)


if @bImprimeEncabezadoyPie is null
	set @bImprimeEncabezadoyPie =0

	
Declare @Fecha as date, @cHora as varchar(10), @folio as int
Declare @nSubTotal numeric (18,2), @nDescto numeric (18,2) , @nTotal numeric (18,2), @nTotalCuenta numeric (18,2), @Totaletra Varchar(max),  @Comentarios varchar(max)
Declare @bImpreso bit =0
Declare @nCambio numeric (18,2)=0, @nPagaCon numeric (18,2)
Declare @nCliente int='', @cCliente Varchar(max)='', @cClienteComanda Varchar(max)='', @Vendedor varchar (max), @nImpuesto numeric (18,2), @nArticulosVendidos int

Set @Fecha=GETDATE()
Set @cHora= (Select CONVERT(VARCHAR(5), GETDATE(), 108) 'hh:mi:ss')    

SELECT  @nTotal=O.nTotal,  @nSubTotal= O.nSubtotal, @nDescto= O.nImporteDescuento , @nImpuesto= O.nimpuestoIVA,
@Comentarios = isnull(o.cComentarios ,''), @nCliente= O.nCliente
FROM VTA_MovimientosVenta As O(NOLOCK) 
Where nVenta=@nFolio

SELECT  @nTotal=O.nTotal,  @nSubTotal= O.nSubtotal, @nDescto= o.nImporteDescuento,
@Comentarios = isnull(o.cComentarios ,''), --, @bImpreso= isnull(o.bCuentaImpresa,0),
@nCliente= O.nCliente, @cClienteComanda= '',
@Vendedor= E.cNombre + ' ' + isnull(cApellidoPaterno,'') + ' ' + isnull(cApellidoMaterno ,'')   
FROM VTA_MovimientosVenta As O(NOLOCK) 
Inner Join Cat_Empleados As E(Nolock)on E.nEmpleado=O.nEmpleado_Registra
Where nVenta=@nFolio

SELECT O.*
Into #VTA_MovimientosVenta
FROM VTA_MovimientosVenta As O(NOLOCK) 
Where nVenta=@nFolio

SELECT O.*
Into #VTA_MovimientosVentaDetalle
FROM VTA_MovimientosVentaDetalle  As O(NOLOCK) 
Where nVenta=@nFolio and O.bActivo = 1 

SELECT @nArticulosVendidos= sum(isnull(nCantidad,0))
From #VTA_MovimientosVentaDetalle

SELECT @cCliente=isnull(cNombreCompleto,'')  FROM CAT_Clientes (NOLOCK)
Where nCliente = @nCliente 

Set @nCambio = Isnull((SELECT isnull(nCambio,0) FROM VTA_MovimientosVenta (NOLOCK) Where nSucursal= @nSucursal and nVenta=@nFolio),0)
Set @nPagaCon  = Isnull((SELECT isnull(nPagaCon ,0) FROM VTA_MovimientosVenta (NOLOCK) Where nSucursal= @nSucursal and nVenta=@nFolio),0)


 Declare @DTTotalLetra as Table (
 nRenglon int,
 cLinea char(50)
 )

 Declare @DetalleOrden as table     
 (nOrden bigint,    
 nRenglonConcepto int,    
 nRenglonModificador int,    
 nCantidad int,    
 cDescripcion varchar(300),
 nImporte numeric (18,2),
 nTotal numeric (18,2),
 bModificador bit
 )   
  
 Declare @DTComentariosOrden as table (
 nRenglon int,
 cLinea char(50)
 )

 Declare @DTFormasPago as table (
 nFormaPago int,
 cFormapago varchar(50), 
 nImporte numeric(18,2),
 nPagaCon numeric(18,2)
 )
  
DECLARE @Ticket as table(   
nRenglon int,  
nRenglonConcepto int,  
nRenglonMod int,  
cLinea char(50),
bLetraGrande bit not null default 0,
bNegrita bit not null default 0,
bCodBarra bit not null default 0
 )
 

Set @Totaletra= dbo.conletra(@nTotal, 'P')

Declare @Index int =0 
Declare @nRenglon int =1 
Declare @Segment varchar(100)

WHILE @Index <= LEN(@Totaletra)
BEGIN
	Set @Segment = SUBSTRING(@Totaletra, @Index, @TotalCaracteres);
   
	Insert Into @DTTotalLetra (nRenglon, cLinea)
	Select @nRenglon, @Segment

	Set @Index = @Index + @TotalCaracteres
	Set @nRenglon = @nRenglon + 1
END


-- permite recorrer o separar en mas de un renglon el comentario general de la orden para que se visualice completo 
-- en caso de exceder el total de caracteres.
Set @Index  =0 
Set @nRenglon  =1 
Set @Segment =''
WHILE @Index <= LEN(@Comentarios  )
BEGIN
	Set @Segment = SUBSTRING(@Comentarios, @Index, @TotalCaracteres);
   
   if @Segment<> '' 
   Begin
		Insert Into @DTComentariosOrden (nRenglon, cLinea)
		Select @nRenglon, @Segment
End 

	Set @Index = @Index + @TotalCaracteres
	Set @nRenglon = @nRenglon + 1
END


-- obtiene las formas de pago  y agrupa los pagos 
Insert Into @DTFormasPago (nFormaPago, cFormaPago, nImporte, nPagaCon )
Select DMC.nFormaPago, FP.cDescripcion as cFormaPago, sum(DMC.nImporte) as nImporte, sum(DMC.nPagaCon ) as nPagaCon
From CAJ_DetalleMovimientosCaja as DMC
Inner Join #VTA_MovimientosVenta MV On Mv.nIDRegistroCaja = DMC.nIDRegistroCaja 
Inner Join CAT_FormasPago as FP (NOLOCK) on FP.nFormaPago =DMC.nFormaPago 
Where DMC.bActivo=1 
Group by DMC.nFormaPago, FP.cDescripcion 


 -- Inserta los conceptos de la orden de una cuenta especifica
Insert Into @DetalleOrden(norden, nRenglonConcepto, nRenglonModificador, nCantidad, cDescripcion, nImporte, nTotal, bModificador)
SELECT VD.nVenta, VD.nRenglon, 1, VD.nCantidad, A.cDescripcion, VD.nPrecioUnitario, VD.nCantidad * VD.nTotal, 0
FROM #VTA_MovimientosVentaDetalle  as VD (NOLOCK)
Inner Join CAT_Articulos  as A (Nolock) on A.nIDArticulo = VD.nIDArticulo 


-- Inserta el encabezado del ticket 
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
Exec RST_EncabezadoTicket @nSucursal

--Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
--Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
--Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket

Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket

--Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
--Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bLetraGrande ) 
--Select @nRenglon, 0,0, Replicate(' ',(@TotalCaracteres - len(@cTipoServicio))/2)  + @cTipoServicio + Replicate(' ',(@TotalCaracteres - len( @cTipoServicio))/2) as  cTicket,1

Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
Select @nRenglon, 0,0, Replicate(' ', @TotalCaracteres) as cTicket 
--union all
--Select @nRenglon +1 , 0,0, Replicate(' ', @TotalCaracteres) as cTicket 


Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bLetraGrande  ) 
Select @nRenglon, 0,0, left('Cliente: ' + Case When Isnull(@cClienteComanda,'')='' then isnull(@cCliente,'') else @cClienteComanda End, @TotalCaracteres ),0

--Select Case When Isnull(@cClienteComanda,'')='' then @cCliente else @cClienteComanda End as Cliente

--Return 


Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0) + 1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
Select @nRenglon, 0,0, left('Atendio: ' + @Vendedor , @TotalCaracteres)

Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
Select @nRenglon, 0,0,'Fecha: ' + ltrim(FORMAT(@Fecha,'dd-MM-yyyy')) + '       Hora: '+ ltrim(@cHora) + ' Hrs.'


if exists(Select top 1 * From @DTComentariosOrden )
Begin

	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
	Select @nRenglon, 0,0, Replicate('-',@TotalCaracteres) as cTicket 

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
	Select @nRenglon, 0,0, left('Comentario: ', @TotalCaracteres)
	
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
	Select @nRenglon + isnull(nRenglon,0),0,0,cLinea as cTicket 
	From @DTComentariosOrden  

End 
		
Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
Select @nRenglon, 0,0, Replicate('=',@TotalCaracteres) as cTicket 

Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
Select @nRenglon, 0,0,left('Cant.' + Replicate(' ', @LongCantidad), @LongCantidad) +
left(' Descripción' + Replicate(' ', @longdescripcion), @longdescripcion) +
Right(Replicate(' ', @LongImporte) + 'Importe' , @LongImporte) 
		
Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
Select @nRenglon, 0,0, Replicate('=',@TotalCaracteres) as cTicket 

-- Inserta el detalle de la orden/cuenta
Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  ( nRenglon, nRenglonConcepto, nRenglonMod, clinea)  
	Select  @nRenglon + row_Number() over(order by nOrden,nRenglonConcepto,nRenglonModificador), nRenglonConcepto, nRenglonModificador, 
	left( right(Replicate(' ',@LongCantidad) + Case When (bmodificador =1 or nCantidad=0) then ' ' else ltrim(nCantidad) End, @LongCantidad) + left( ' ' + cDescripcion +  REPLICATE(' ',@LongDescripcion ), @LongDescripcion) + RIGHT(REPLICATE(' ', @LongImporte )+CONVERT(varchar(20),CONVERT(decimal(18,2),nTotal)),@LongImporte), @TotalCaracteres )  
	From @DetalleOrden 
	Order by  nOrden,nRenglonConcepto, nRenglonModificador
		
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
	Select @nRenglon, 0,0, Replicate('=',@TotalCaracteres) as cTicket 
		
 	
	if @nDescto >0 
	Begin
		Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
		Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
		Select @nRenglon, 0,0, right(right(Replicate(' ',@TotalCaracteres - @LongImporte ) + 'SUBTOTAL: ',@TotalCaracteres - @LongImporte )  + RIGHT(REPLICATE(' ',@LongImporte )+CONVERT(varchar(20),CONVERT(decimal(18,2),@nSubTotal)),@LongImporte),@TotalCaracteres ) as cTicket 
	
		Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
		Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
		Select @nRenglon, 0,0, right(right(Replicate(' ',@TotalCaracteres - @LongImporte) + 'DESCTO: ',@TotalCaracteres - @LongImporte)  + RIGHT(REPLICATE(' ', @LongImporte) +CONVERT(varchar(10),CONVERT(decimal(18,2),@nDescto )),@LongImporte),@TotalCaracteres) as cTicket 
	End 

	

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bNegrita )
	Select @nRenglon, 0,0, right(right(Replicate(' ',@TotalCaracteres - @LongImporte) + 'SUBTOTAL: ',@TotalCaracteres - @LongImporte)  + RIGHT(REPLICATE(' ',@LongImporte)+CONVERT(varchar(20),CONVERT(decimal(18,2),@nSubTotal )),@LongImporte),@TotalCaracteres) as cTicket,1
	
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bNegrita )
	Select @nRenglon, 0,0, right(right(Replicate(' ',@TotalCaracteres - @LongImporte) + 'DESCTO: ',@TotalCaracteres - @LongImporte)  + RIGHT(REPLICATE(' ',@LongImporte)+CONVERT(varchar(20),CONVERT(decimal(18,2),@nDescto )),@LongImporte),@TotalCaracteres) as cTicket,1
	
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bNegrita )
	Select @nRenglon, 0,0, right(right(Replicate(' ',@TotalCaracteres - @LongImporte) + 'IMPUESTO: ',@TotalCaracteres - @LongImporte)  + RIGHT(REPLICATE(' ',@LongImporte)+CONVERT(varchar(20),CONVERT(decimal(18,2),@nImpuesto  )),@LongImporte),@TotalCaracteres) as cTicket,1
	

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bNegrita )
	Select @nRenglon, 0,0, right(right(Replicate(' ',@TotalCaracteres - @LongImporte) + 'TOTAL: ',@TotalCaracteres - @LongImporte)  + RIGHT(REPLICATE(' ',@LongImporte)+CONVERT(varchar(20),CONVERT(decimal(18,2),@nTotal)),@LongImporte),@TotalCaracteres) as cTicket,1
	
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
	Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket 

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bNegrita )
	Select @nRenglon, 0,0,  '#Articulos: ' + ltrim(@nArticulosVendidos ),1
	
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
	Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket 

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
	Select @nRenglon + isnull(nRenglon,0),0,0,cLinea as cTicket 
	From @DTTotalLetra 

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
	Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket 

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea )
	Select @nRenglon, 0,0, 'Forma de pago:' as cTicket 

	--Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1 
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea)  
	Select  @nRenglon + row_Number()over(order by nFormaPago),0,0,
	right(Replicate(' ',31) + F.cFormapago + ':',31 ) +
	right(Replicate(' ',@LongImporte ) + CONVERT(varchar(20),CONVERT(decimal(18,2),nImporte )),@LongImporte)
	From @DTFormasPago as F 
	Order by  nFormaPago 

	if @nPagaCon  <>0
	Begin
		Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
		Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
		Select @nRenglon,0,0,'PagaCon:  ' +  CONVERT(varchar(20),CONVERT(decimal(18,2),@nPagaCon,0)) as cTicket 
	End 

	if @nCambio <>0
	Begin	
		Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
		Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
		Select @nRenglon, 0,0, 'Cambio:  ' +  CONVERT(varchar(20),CONVERT(decimal(18,2),@nCambio  )) as cTicket 
	End 
	
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
	Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket 
	
	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
	Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket 

	Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1
	Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea, bLetraGrande, bCodBarra )
	--Select @nRenglon, 0,0, Case when @nOrden is null then '' else 'No. ORDEN: ' + ltrim(@nOrden) End 
	select  @nRenglon + 1, 1, 1, Replicate(' ',(@TotalCaracteres  - len('#ORDEN'))/2) + '#ORDEN' + Replicate(' ',(@TotalCaracteres - len('#ORDEN'))/2), 0,0
	union all
	select  @nRenglon + 2, 1, 1,'   '+ltrim(@nFolio), 0,1

Set @nRenglon= isnull((Select max(nRenglon) From @Ticket),0)+1  
Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
Select @nRenglon, 0,0, Replicate(' ',@TotalCaracteres) as cTicket 	

Insert Into @Ticket  (nRenglon, nRenglonConcepto, nRenglonMod, clinea ) 
Exec RST_PieDeTicket @nSucursal


Select * From @Ticket 

End 
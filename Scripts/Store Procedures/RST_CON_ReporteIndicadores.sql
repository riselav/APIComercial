sp_eliminastore 'RST_CON_ReporteIndicadores'
GO
-- Select dbo.NumeroFecha_Fn(45865)
-- Select dbo.FechaNumero_Fn('20250701')
-- Exec RST_CON_ReporteIndicadores 1, 45839, 45865 , 6 
Create procedure RST_CON_ReporteIndicadores (@nSucursal int,@FechaNumeroInicial int=0, @FechaNumeroFinal int=0,@nTipoDetalle tinyint=0)  
As  
Begin  
  
Set NoCount on   

Declare @nEstatusPagado int = isnull((Select nCodigo From CAT_Catalogos Where cNombre ='CAT_EstatusOrden' and cDescripcion ='PAGADO'),0) 
Declare @nEstatusLiberado int = isnull((Select nCodigo From CAT_Catalogos Where cNombre ='CAT_EstatusOrden' and cDescripcion ='LIBERADO'),0) 
Declare @TotalCaracteres int = 40  
Declare @LongImporte int = 9  
Declare @LongCantidad int = 4  
Declare @LongDescripcion int = (@TotalCaracteres - @LongImporte - @LongCantidad)  

Declare @nFormaPago int = 1   
Declare @nFormaPagoEfectivo int = 1  
  
-- tipos de movimientos de caja  
Declare @nTipoRegistroApertura int =1   
Declare @nTipoRegistroRetiro int = 2  
Declare @nTipoRegistroIngreso int =3  
Declare @nTipoRegistroGasto int = 4  
Declare @nTipoRegistroPago int = 5   
Declare @nTipoRegistroCorte int = 6  
Declare @bCr_Simul bit=Isnull((Select isnull(cValor,0) from GRAL_Parametros Where cParametro='CR_Simul'),0)  
Declare @nTotalVenta numeric(18,2)  
Declare @nTotalOrdenes int=0

Declare @vnFecha int,@vnFechaFinal int

SET @vnFecha=@FechaNumeroInicial
SET @vnFechaFinal=@FechaNumeroFinal

Select C.*   
Into #CAJ_CortesCaja  
From CAJ_CortesCaja as c   
JOIN CAJ_RegistrosAperturaCaja AP (NOLOCK) ON C.nIDApertura=AP.nIDApertura
Where 1=1 
AND dbo.FechaNumero_Fn(AP.dFecha) BETWEEN @vnFecha AND @vnFechaFinal
    
Select c.*   
Into #CAJ_DetalleCorteCaja  
From CAJ_DetalleCorteCaja as c   
Join #CAJ_CortesCaja CC ON CC.nIDCorteCaja=c.nIDCorteCaja
  
SELECT AP.*
Into #CAJ_RegistrosAperturaCaja  
FROM CAJ_RegistrosAperturaCaja AP (NOLOCK)  
JOIN #CAJ_CortesCaja CC ON CC.nIDApertura=AP.nIDApertura

DECLARE @bTodo bit=~@bCr_Simul -- Neg.de @bCr_Simul

Select MC.*  
Into #CAJ_MovimientosCaja  
From CAJ_MovimientosCaja MC(Nolock)
Join #CAJ_CortesCaja CC ON CC.nIDCorteCaja=MC.nIDCorteCaja
Where MC.bActivo =1
AND ISNULL(MC.bRegistroEspecial,0)= CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END

Select MDC.*   
Into #CAJ_DetalleMovimientosCaja  
From CAJ_DetalleMovimientosCaja as MDC (Nolock)  
Inner Join #CAJ_MovimientosCaja as MC (Nolock) on MC.nIDRegistroCaja =mdc.nIDRegistroCaja 
Where MDC.bActivo =1

/*
Select C.nIDCorteCaja,
1 as nTipo, -1 as nFormaPago, Cast('DOTACION INICIAL' as Varchar(max))as cFormaPago, 
SUM(C.nDotacionInicial) As nImporte, SUM(C.nDotacionInicial) as nImporteUsuario  -- , Cast (0 as numeric(18,2)) as nDiferencia  
Into #ConcentradoCaja  
FROM #CAJ_CortesCaja As C (NOLOCK)  
Group by C.nIDCorteCaja*/

-- Obtiene solo los pagos de las ventas  
SELECT MC.nTipoRegistroCaja, MC.nIDCorteCaja, MC.nIDApertura, 
OCE.nOrden, OCE.nCuenta, MDC.nFormaPago, FP.cDescripcion as cFormaPago, 
nImporte=   CASE WHEN @bTodo=1 THEN isnull(MDC.nImporte_Respaldo, MDC.nImporte) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE MDC.nImporte END END, -- <- Cr
OE.nTipoServicio, trim(C.cDescripcion) as cTipoServicio, OE.nEmpleadoAbreMesa as nEmpleado, 
cEmpleado=Cast( E.cNombre + ' ' + isnull(cApellidoMaterno,'') + ' ' + isnull(cApellidoMaterno ,'') as Varchar(300)),  
nTotal=		CASE WHEN @bTodo=1 THEN isnull(MDC.nImporte_Respaldo, MDC.nImporte) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE MDC.nImporte END END, -- <- Cr 
nDescuento= CASE WHEN @bTodo=1 THEN isnull(OE.nDescuento_Respaldo,OE.nDescuento) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE OE.nDescuento END END, -- <- Cr
MC.bRegistroEspecial, -- <- Cr
AP.dFecha,OE.nCliente,OE.nMesa
Into #MovtosPagoOrden  
FROM #CAJ_MovimientosCaja As MC(NOLOCK)  
Inner Join CAJ_RegistrosAperturaCaja AP (NOLOCK) ON AP.nIDApertura=MC.nIDApertura
Inner Join #CAJ_DetalleMovimientosCaja as MDC (NOLOCK) on MDC.nIDRegistroCaja = MC.nIDRegistroCaja And MDC.bActivo =1 and MC.nTipoRegistroCaja = 5 -- 5=Tipo de registro Pago  
Inner Join REG_OrdenesCuentasEncabezado as OCE (Nolock) on OCE.nOrden = MDC.nOrden and OCE.nCuenta =MDC.nCuenta and OCE.bActivo =1  
Inner Join CAT_FormasPago  as FP (Nolock) on FP.nFormaPago =MDC.nFormaPago   
Inner Join REG_OrdenesEncabezado  as OE (Nolock) on OE.norden = OCE.nOrden and OE.nEstatus IN(@nEstatusPagado,@nEstatusLiberado)
Left Join CAT_Empleados  as E (Nolock) on E.nEmpleado = OE.nEmpleadoAbreMesa   
Inner Join CAT_Catalogos as C (Nolock) on C.cNombre ='CAT_TipoServicio' and C.nCodigo = OE.nTipoServicio  
Where MC.bActivo =1 

/*
Insert Into #ConcentradoCaja  
Select PO.nIDCorteCaja, 
1 as nTipo, 0 as nFormaPago, Cast('TOTAL DE VENTA' as Varchar(max))as cFormaPago, Sum(nImporte) As nImporte, Sum(nImporte) as nImporteUsuario
From #MovtosPagoOrden PO -- <- Cr
Group by PO.nIDCorteCaja

Insert Into #ConcentradoCaja  
Select CC.nIDCorteCaja,  
2 as nTipo, FP.nFormaPago, '     - ' + ISNULL(cFormaPago,FP.cDescripcion), Sum(ISNULL(nImporte,0)) As nImporte, 0 as nImporteUsuario
From #CAJ_DetalleCorteCaja CC
Left Join CAT_FormasPago FP (NOLOCK) ON FP.nFormaPago=CC.nFormaPago
LEFT JOIN #MovtosPagoOrden PO ON CC.nIDCorteCaja= PO.nIDCorteCaja
	AND CC.nFormaPago=PO.nFormaPago
Group by CC.nIDCorteCaja, FP.nFormaPago, cFormaPago,FP.cDescripcion  

UPDATE CC SET CC.nImporteUsuario= CASE WHEN @bTodo=1 THEN 
									    --isnull(DC.nImporteUsuario_Respaldo,DC.nImporteUsuario)
										CASE WHEN DC.nImporteUsuario_Respaldo IS NULL THEN
											      DC.nImporteUsuario
										ELSE
											DC.nImporteUsuario_Respaldo- 
											   CASE WHEN DC.nImporteUsuario_Respaldo=DC.nImporteCorte_Respaldo AND DC.nDiferencia_Respaldo IS NOT NULL THEN
													DC.nDiferencia_Respaldo
											   ELSE
													0
											   END
										END
								   ELSE
										DC.nImporteUsuario											 
								   END
FROM #ConcentradoCaja CC
JOIN #CAJ_DetalleCorteCaja DC ON CC.nFormaPago=DC.nFormaPago
	AND CC.nIDCorteCaja=DC.nIDCorteCaja
WHERE nTipo=2	

Insert Into #ConcentradoCaja  
Select C.nIDCorteCaja, 
3 as nTipo, 1 as nFormaPago , '(+) OTROS INGRESOS',
nImporte= SUM(CASE WHEN @bTodo=1 THEN ISNULL(nTotalIngresos_Respaldo,nTotalIngresos) ELSE nTotalIngresos END), 
nImporteUsuario= SUM(CASE WHEN @bTodo=1 THEN ISNULL(nTotalIngresos_Respaldo,nTotalIngresos) ELSE nTotalIngresos END)
From #CAJ_CortesCaja as C  
Group by nIDCorteCaja
  
Insert Into #ConcentradoCaja  
Select C.nIDCorteCaja,
4 as nTipo, -2 as nFormaPago , '(-) RETIROS',SUM(-1 * nTotalRetiros) As nImporte, SUM(-1 * nTotalRetiros) as nImporteUsuario  
From #CAJ_CortesCaja as C
Group by nIDCorteCaja
  
Insert Into #ConcentradoCaja  
Select C.nIDCorteCaja, 
5 as nTipo, -2 as nFormaPago, '(-) GASTOS',SUM(-1 * nTotalGastos) As nImporte, SUM(-1 *  nTotalGastos) as nImporteUsuario  
From #CAJ_CortesCaja as C  
Group by nIDCorteCaja
*/

Select
OE.nOrden, OE.nImporteServDom, OD.nConcepto, CV.cDescripcion as cConcepto, OD.nCantidad, OD.nImporte as nImporteConcepto, OD.nTotal as nTotalConcepto, OD.nEstacionCocina  as nEstacionCocina,
EC.cDescripcion  as cEstacionCocina, OD.nCocina, C.cDescripcion as cCocina, OD.nSubtotal, Cast(0 as numeric(18,4)) as nServicioDomicilio,
Cast(0 as numeric(18,2)) as nTotalCompleto,TCn.cDescripcion as cCategoria
Into #DetalleVenta
From REG_OrdenesEncabezado as OE (Nolock) 
Inner Join (Select nOrden From #MovtosPagoOrden Group by nOrden ) As P On P.nOrden =OE.nOrden 
Inner Join CAT_Empleados  as E (Nolock) on E.nEmpleado = OE.nEmpleadoAbreMesa   
Inner Join REG_OrdenesDetalle as OD (Nolock) on OD.nOrden = OE.nOrden  and OD.bActivo =1
	  And OD.bCancelado= CASE WHEN @bTodo=1 THEN OD.bCancelado ELSE 0 END -- <--- Cr
Inner Join CAT_ConceptosVenta  as CV (Nolock) on CV.nConceptoVenta = OD.nConcepto   
Inner Join CAT_TiposConceptos TCn (NOLOCK) ON TCn.nTipoConcepto=CV.nTipoConcepto
Inner Join CAT_EstacionesCocinas as EC (Nolock) on EC.nEstacionCocina = OD.nEstacionCocina 
left Join CAT_Cocinas  as C (Nolock) On C.nCocina =OD.nCocina 

Set @nTotalVenta = Isnull((Select sum(nImporte) as nImporte From #MovtosPagoOrden),0) -- Total completo para el resto de graficas que lo ocupan

Select nOrden, Count(norden) As nConceptos
	Into #ConceptosporOrden
	From #DetalleVenta
	Group by nOrden

		
Set @nTotalOrdenes = Isnull((Select count(norden) From #ConceptosporOrden),0)

CREATE TABLE #MesesDelAño (
		numMes INT,
		mes VARCHAR(20)
	);

INSERT INTO #MesesDelAño (numMes, mes)
VALUES
(1, 'enero'),
(2, 'febrero'),
(3, 'marzo'),
(4, 'abril'),
(5, 'mayo'),
(6, 'junio'),
(7, 'julio'),
(8, 'agosto'),
(9, 'septiembre'),
(10, 'octubre'),
(11, 'noviembre'),
(12, 'diciembre');

IF @nTipoDetalle=0
BEGIN
	Select nEstacionCocina, norden --COUNT(DISTINCT nOrden) AS nTotalOrdenes
	Into #OrdenesCocina
	From #DetalleVenta
	
	-- Establece el total de la venta del corte de caja o dia  
	Select nEmpleado,sum(nImporte) as nImporte,ROW_NUMBER() OVER(ORDER BY sum(nImporte) DESC) as nRenglon 
	Into #MovtosPagoOrdenGroup 
	From #MovtosPagoOrden 
	GROUP BY nEmpleado ORDER BY sum(nImporte) DESC

	DELETE #MovtosPagoOrdenGroup WHERE nRenglon>5

	Select nCliente,COUNT(1) as nCantidad,ROW_NUMBER() OVER(ORDER BY COUNT(1) DESC) as nRenglon 
	Into #MovtosPagoOrdenClienteGroup 
	From #MovtosPagoOrden 
	GROUP BY nCliente ORDER BY COUNT(1) DESC

	Select nMesa,COUNT(1) as nCantidad,ROW_NUMBER() OVER(ORDER BY COUNT(1) DESC) as nRenglon 
	Into #MovtosPagoOrdenMesasGroup
	From #MovtosPagoOrden 
	GROUP BY nMesa ORDER BY COUNT(1) DESC

	DECLARE @nClientes int=(SELECT COUNT(1) FROM #MovtosPagoOrdenClienteGroup)
	DECLARE @nMesasOcupadas int=(SELECT COUNT(1) FROM #MovtosPagoOrdenMesasGroup)

	----Set @nTotalVenta = Isnull((Select sum(nImporte) as nImporte From #MovtosPagoOrdenGroup),0)  -- total solo del top 5
	Declare @nTotalSinServicio as numeric (18,2)=0
	Declare @nServDom as numeric (8,2)

	Set @nServDom= IsNull((Select sum(isnull(nImporteServDom,0)) From (Select nOrden, min(nImporteServDom) as nImporteServDom  From #DetalleVenta Group by nOrden) as SD),0)

	set @nTotalSinServicio= @nTotalVenta - @nServDom

	--Select @nTotalVenta as '@nTotalVenta', @nTotalOrdenes as '@nTotalOrdenes', @nServDom as '@nServDom', @nTotalSinServicio as '@nTotalSinServicio'

	Select  nEmpleado,  cEmpleado, count(nOrden) as nTotalOrdenes, sum(nTotal) as nTotal,
	Case when @nTotalVenta =0 then 0 else Cast(sum(nTotal)/@nTotalVenta as numeric(18,2)) End as nPorcVentaProporcional
	Into #VentaEmpleados 
	From (Select nEmpleado, cEmpleado, nOrden, Sum(nTotal) as nTotal 
	From #MovtosPagoOrden Group by nEmpleado, cEmpleado, nOrden, cEmpleado)as MP
	Group by nEmpleado,  cEmpleado

	--Update VE Set VE.nTotalOrdenes = ot.nTotalOrdenes, ve.nPorcVentaProporcional = Cast((nTotal /@nTotalVenta) as numeric(18,4))  
	--From #VentaEmpleados as VE   
	--Inner Join (Select nEmpleado, cEmpleado, Count(nOrden) as nTotalOrdenes  
	--            From #MovtosPagoOrden  
	--   Group by nEmpleado, cEmpleado) as OT on OT.nEmpleado= VE.nEmpleado  

	DECLARE @nTotalVentasFacturadas decimal(18,4)=(  
		Select 
			--ISNULL(COUNT(1),0) as Cantidad, 
			ISNULL(SUM(CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END),0) as Total
		from REG_OrdenesEncabezado Ord(NOLOCK)
		join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
		Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
		where 1=1 --nIDApertura= @IDApertura
			and nFactura IS NOT NULL and Ord.nEstatus<>6 and c.bActivo=1 AND isnull(C.bCancelado,0)=0
	)

	DECLARE @nIngresos decimal(18,4)=(
		SELECT SUM(MC.nImporte) as nImporte
		FROM CAJ_MovimientosCaja MC (NOLOCK)
		JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
		JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
		WHERE MC.bActivo=1 AND MC.nEfecto=1
		AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
	)

	IF @nIngresos IS NULL SET @nIngresos=0

	DECLARE @nEgresos decimal(18,4)=(
		SELECT SUM(MC.nImporte) as nImporte
		FROM CAJ_MovimientosCaja MC (NOLOCK)
		JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
		LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
		WHERE MC.bActivo=1 AND MC.nEfecto=-1
			--AND MC.nTipoRegistroCaja=2 -- Retiros de caja 
	)

	IF @nEgresos IS NULL SET @nEgresos=0

	DECLARE @nTicketPromedio decimal(18,2)= (SELECT CASE WHEN @nTotalOrdenes=0 THEN 0.00 ELSE Cast(ISNULL(@nTotalVenta,0) / @nTotalOrdenes as numeric(18,2)) END)

	DECLARE @nTotalMesas int=(SELECT COUNT(1) FROM CAT_Mesas (NOLOCK) WHERE bActivo=1 AND nSucursal=@nSucursal )
	DECLARE @occupancyRate decimal(18,4)

	SET @occupancyRate = (CAST(@nMesasOcupadas AS DECIMAL(18,4)) / @nTotalMesas)

	-- KPI Summary
	DECLARE @netIncome decimal(18,4) =(SELECT @nTotalVenta+ @nIngresos-@nEgresos)

	SELECT totalSales=@nTotalVenta,totalSalesPreviousPeriod=0.00,
		   invoicedSales=@nTotalVentasFacturadas, invoicedSalesPreviousPeriod=0.00,
		   uninvoicedSales=@nTotalVenta-@nTotalVentasFacturadas,uninvoicedSalesPreviousPeriod=0.00,
		   netIncome=@netIncome,netIncomePreviousPeriod=0.00,
		   totalExpenses=@nEgresos,totalExpensesPreviousPeriod=0.00,
		   numberOfCustomers=@nClientes,numberOfCustomersPreviousPeriod=0.00,
		   averageTicket=@nTicketPromedio,averageTicketPreviousPeriod=0.00,
		   occupancyRate=@occupancyRate,occupancyRatePreviousPeriod=0.00

	-- Ventas por Día
	SET DATEFIRST 1; -- Establece el lunes como el primer día de la semana

	SELECT DATEPART(WEEKDAY, dFecha) as nDiaSemana,FORMAT(dFecha, 'dddd', 'es-MX') as Date,SUM(nImporte) as TotalSales
	FROM #MovtosPagoOrden
	GROUP BY DATEPART(WEEKDAY, dFecha),FORMAT(dFecha, 'dddd', 'es-MX')
	ORDER BY DATEPART(WEEKDAY, dFecha)

	-- Ventas por Categoría
	Select P.cCategoria as CategoryName,P.nTotalConcepto as TotalSales
	From (Select cCategoria, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
		  (sum(nCantidad)* min(nImporteConcepto))  as nTotal,
		  (sum(nTotalConcepto)) as nTotalConcepto
		   From #DetalleVenta
		   Group by cCategoria) as P
	Order by P.cCategoria

	-- Ingresos Vs Gastos
	Create table #IncomeVsExpenses(numMes int,mes varchar(100),income decimal(18,2), expenses decimal(18,2))

	SET LANGUAGE Spanish;

	-- ** Egresos

	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #Egresos
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=-1
		--AND MC.nTipoRegistroCaja=2 -- Retiros de caja 
	GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	-- ** Ingresos

	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #Ingresos
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=1
		AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
	GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)
	
	INSERT INTO #IncomeVsExpenses (numMes, mes, income, expenses)
	SELECT
		M.numMes,
		M.mes,
		ISNULL(I.nImporte, 0) AS income,
		ISNULL(E.nImporte, 0) AS expenses
	FROM #MesesDelAño M
	LEFT JOIN #Ingresos I ON M.numMes = I.numMes
	LEFT JOIN #Egresos E ON M.numMes = E.numMes
	ORDER BY M.numMes;

	Select mes, income, expenses FROM #IncomeVsExpenses

	-- Venta Facturada vs No Facturada
	Create table #InvoicedVsUninvoiced(numMes int,mes varchar(100),invoiced decimal(18,2), uninvoiced decimal(18,2))

	Select MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes, 
	ISNULL(SUM(
	Case when nFactura IS NOT NULL THEN
			CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END 
		 else
			0
		 end),0) as Facturado,
	ISNULL(SUM(
	Case when nFactura IS NULL THEN
			C.nTotal 
		 else
			0
		 end),0) as NoFacturado
	Into #FactVsNoFact
	from REG_OrdenesEncabezado Ord(NOLOCK)
	join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
	Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
	where 1=1
		and Ord.nEstatus<>6 and c.bActivo=1 AND isnull(C.bCancelado,0)=0
	Group by MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	INSERT INTO #InvoicedVsUninvoiced (numMes, mes, invoiced, uninvoiced)
	SELECT
		M.numMes,
		M.mes,
		ISNULL(I.Facturado, 0) AS Facturado,
		ISNULL(I.NoFacturado, 0) AS NoFacturado
	FROM #MesesDelAño M
	LEFT JOIN #FactVsNoFact I ON M.numMes = I.numMes
	ORDER BY M.numMes;

	Select mes,invoiced, uninvoiced FROM #InvoicedVsUninvoiced

	-- Ventas por Forma de Pago
	Create table #SalesByPaymentMethodDto(nFormaPago int,PaymentMethod varchar(200),TotalSales decimal(18,2))

	Select nFormaPago,cDescripcion as cFormaPago
	into #CAT_FormasPago
	FROM CAT_FormasPago
	WHERE bActivo=1

	Select  
	nFormaPago, cFormaPago, sum (nImporte) as nImporte
	into #FormasPagoMovtos
	From #MovtosPagoOrden   
	Group by nFormaPago, cFormaPago 
	order by nImporte desc

	INSERT INTO #SalesByPaymentMethodDto(nFormaPago, PaymentMethod, TotalSales)
	SELECT
		M.nFormaPago,
		M.cFormaPago,
		ISNULL(I.nImporte, 0) AS Total
	FROM #CAT_FormasPago M
	LEFT JOIN #FormasPagoMovtos I ON M.nFormaPago = I.nFormaPago
	ORDER BY M.nFormaPago;

	Select PaymentMethod, TotalSales FROM #SalesByPaymentMethodDto

	-- Ventas por Tipo de Servicio
	Create table #TiposServicio (nTipoServicio int,cTipoServicio varchar(200))

	Insert into #TiposServicio
	Select nCodigo,LTRIM(RTRIM(cDescripcion)) FROM CAT_Catalogos (NOLOCK) WHere cNombre= 'CAT_TipoServicio'

	Select 
	nTipoServicio, cTipoServicio, sum (nImporte) as nImporte
	into #TiposServ
	From #MovtosPagoOrden   
	Group by
	nTipoServicio, cTipoServicio 
	Order by nImporte desc

	Create table #SalesByServiceTypeDto(ServiceType varchar(200),TotalSales decimal(18,2))

	INSERT INTO #SalesByServiceTypeDto(ServiceType, TotalSales)
	SELECT
		M.cTipoServicio,
		ISNULL(I.nImporte, 0) AS Total
	FROM #TiposServicio M
	LEFT JOIN #TiposServ I ON M.nTipoServicio = I.nTipoServicio
	ORDER BY M.nTipoServicio;

	Select ServiceType, TotalSales FROM #SalesByServiceTypeDto

	-- Ventas por Estación de Cocina
	Create table #EstacionesCocina (nEstacionCocina int,cEstacionCocina varchar(200))

	Insert into #EstacionesCocina
	Select nEstacionCocina,cDescripcion
	FROM CAT_EstacionesCocinas (NOLOCK)
	Where bActivo=1

	Select nEstacionCocina,cEstacionCocina,Cast (sum(nTotalConcepto + nServicioDomicilio ) as numeric(18,2)) as nImporte
	into #Estaciones
	From #DetalleVenta 
	Group by nEstacionCocina,cEstacionCocina
	Order by nEstacionCocina

	Create table #SalesByKitchenStationDto(StationName varchar(200),TotalSales decimal(18,2))

	INSERT INTO #SalesByKitchenStationDto(StationName, TotalSales)
	SELECT
		M.cEstacionCocina,
		ISNULL(I.nImporte, 0) AS Total
	FROM #EstacionesCocina M
	LEFT JOIN #Estaciones I ON M.nEstacionCocina = I.nEstacionCocina
	ORDER BY M.nEstacionCocina;

	Select StationName, TotalSales FROM #SalesByKitchenStationDto

	-- Reporte de conceptos con más venta 
	Select top 10 P.cConcepto as DishName,P.nCantidad as Quantity 
	From 
	(Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
	(sum(nCantidad)* min(nImporteConcepto))  as nTotal
	--sum(nImporteConcepto*nCantidad) as nTotal
	From #DetalleVenta
	Group by cConcepto) as P
	Order by nCantidad desc 

	-- Reporte de conceptos más valiosos 
	Select top 10 P.cConcepto as DishName,P.nTotal as RevenueOrProfit 
	From 
	(Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
	(sum(nCantidad)* min(nImporteConcepto))  as nTotal
	--sum(nImporteConcepto*nCantidad) as nTotal
	From #DetalleVenta
	Group by cConcepto) as P
	Order by nTotal desc
End

IF @nTipoDetalle=1
BEGIN
	-- Detalle de Ventas por Categoría
	Select P.cCategoria as name,P.nTotalConcepto as valor
	From (Select cCategoria, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
		  (sum(nCantidad)* min(nImporteConcepto)) as nTotal,
		  (sum(nTotalConcepto)) as nTotalConcepto
		   From #DetalleVenta
		   Group by cCategoria) as P
	Order by P.cCategoria

	Select P.cCategoria as category,
	 P.nTotalConcepto as sales, 
	CONVERT(decimal(18,2),(P.nTotalConcepto/@nTotalVenta)*100) as porcentaje,
	 P.products,
	CONVERT(decimal(18,2),(P.nTotalConcepto/@nTotalOrdenes)) as averageTicket,
	0.00 as trend,CONVERT(bit,1) as trendPositive
	From (Select cCategoria, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
		  (sum(nCantidad)* min(nImporteConcepto)) as nTotal,
		  (sum(nTotalConcepto)) as nTotalConcepto,
		   count(nConcepto) as products
		   From #DetalleVenta
		   Group by cCategoria) as P
	Order by P.cCategoria
END

IF @nTipoDetalle=2
BEGIN
	Create table #IncomeVsExpensesDetail(numMes int,mes varchar(100),income decimal(18,2), expenses decimal(18,2),gananciaNeta decimal(18,2))

	SET LANGUAGE Spanish;

	-- ** Egresos

	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #EgresosDetalle
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=-1
		--AND MC.nTipoRegistroCaja=2 -- Retiros de caja 
	GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	-- ** Ingresos

	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #IngresosDetalle
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=1
		AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
	GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	INSERT INTO #IncomeVsExpensesDetail (numMes, mes, income, expenses,gananciaNeta)
	SELECT
		M.numMes,
		M.mes,
		ISNULL(I.nImporte, 0) AS income,
		ISNULL(E.nImporte, 0) AS expenses,
		ISNULL(I.nImporte, 0)-ISNULL(E.nImporte, 0)
	FROM #MesesDelAño M
	LEFT JOIN #IngresosDetalle I ON M.numMes = I.numMes
	LEFT JOIN #EgresosDetalle E ON M.numMes = E.numMes
	ORDER BY M.numMes;

	Select mes, income, expenses,gananciaNeta FROM #IncomeVsExpensesDetail

	Select mes, income, expenses,gananciaNeta as netProfit, 
	Convert(decimal(18,2),Case When income=0 THEN 0 ELSE (gananciaNeta/income)*100 END) as margin,
	0.00 as trend,CONVERT(bit,1) as trendPositive
	FROM #IncomeVsExpensesDetail
END

IF @nTipoDetalle=3
BEGIN
	-- Venta Facturada vs No Facturada
	Create table #InvoicedVsUninvoicedDetail(numMes int,mes varchar(100),ventasFacturadas decimal(18,2), ventasNoFacturadas decimal(18,2),ventaTotal decimal(18,2))

	Select MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes, 
	ISNULL(SUM(
	Case when nFactura IS NOT NULL THEN
			CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END 
		 else
			0
		 end),0) as Facturado,
	ISNULL(SUM(
	Case when nFactura IS NULL THEN
			C.nTotal 
		 else
			0
		 end),0) as NoFacturado,
	ISNULL(SUM(C.nTotal),0) as nVentaTotal
	Into #FactVsNoFactDetalle
	from REG_OrdenesEncabezado Ord(NOLOCK)
	join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
	Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
	where 1=1
		and Ord.nEstatus<>6 and c.bActivo=1 AND isnull(C.bCancelado,0)=0
	Group by MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	INSERT INTO #InvoicedVsUninvoicedDetail (numMes, mes, ventasFacturadas, ventasNoFacturadas,ventaTotal)
	SELECT
		M.numMes,
		M.mes,
		ISNULL(I.Facturado, 0) AS Facturado,
		ISNULL(I.NoFacturado, 0) AS NoFacturado,
		ISNULL(I.nVentaTotal, 0) AS nVentaTotal
	FROM #MesesDelAño M
	LEFT JOIN #FactVsNoFactDetalle I ON M.numMes = I.numMes
	ORDER BY M.numMes;

	Select mes,ventasFacturadas, ventasNoFacturadas FROM #InvoicedVsUninvoicedDetail

	Select mes,ventaTotal,
	ventasFacturadas, Case When ventaTotal =0 Then 0 Else Convert(decimal(18,2), (ventasFacturadas/ventaTotal)*100) End as invoicedPercentage,
	ventasNoFacturadas,
	ventasFacturadas+ventasNoFacturadas as Total,Case When ventaTotal =0 Then 0 Else 100-Convert(decimal(18,2), (ventasFacturadas/ventaTotal)*100) End as uninvoicedPercentage	
	FROM #InvoicedVsUninvoicedDetail
END

IF @nTipoDetalle=4
BEGIN
-- Ventas por Forma de Pago
	Create table #SalesByPaymentMethodDetail(nFormaPago int,PaymentMethod varchar(200),TotalSales decimal(18,2),Cant int)

	Select nFormaPago,cDescripcion as cFormaPago
	into #CAT_FormasPago2
	FROM CAT_FormasPago
	WHERE bActivo=1
	
	Select  
	nFormaPago, cFormaPago, sum (nImporte) as nImporte,COUNT(1) as nCant
	into #FormasPagoMovtosDetalle
	From #MovtosPagoOrden   
	Group by nFormaPago, cFormaPago 
	order by nImporte desc
	
	INSERT INTO #SalesByPaymentMethodDetail(nFormaPago, PaymentMethod, TotalSales,Cant)
	SELECT
		M.nFormaPago,
		M.cFormaPago,
		ISNULL(I.nImporte, 0) AS Total,
		ISNULL(I.nCant, 0) AS Cant
	FROM #CAT_FormasPago2 M
	LEFT JOIN #FormasPagoMovtosDetalle I ON M.nFormaPago = I.nFormaPago
	ORDER BY M.nFormaPago;

	Select PaymentMethod, TotalSales FROM #SalesByPaymentMethodDetail

	DECLARE @nTotalFormasPago int=(SELECT SUM(TotalSales) FROM #SalesByPaymentMethodDetail)

	Select PaymentMethod, TotalSales as sales, 
		CASE WHEN @nTotalFormasPago=0 THEN 0 ELSE CONVERT(decimal(18,2),TotalSales/@nTotalFormasPago)*100 END as porcentaje,
		cant as transactions,
		CONVERT(decimal(18,2),(TotalSales/@nTotalOrdenes)) as averageTicket,
		0.00 as trend,CONVERT(bit,1) as trendPositive
	FROM #SalesByPaymentMethodDetail
END

IF @nTipoDetalle=5
BEGIN
	-- Ventas por Tipo de Servicio
	Create table #TiposServicio2 (nTipoServicio int,cTipoServicio varchar(200))

	Insert into #TiposServicio2
	Select nCodigo,LTRIM(RTRIM(cDescripcion)) FROM CAT_Catalogos (NOLOCK) WHere cNombre= 'CAT_TipoServicio'
	
	Select 
	nTipoServicio, cTipoServicio, sum (nImporte) as nImporte,COUNT(1) as Cant
	into #TiposServDetalle
	From #MovtosPagoOrden   
	Group by
	nTipoServicio, cTipoServicio 
	Order by nImporte desc

	Create table #SalesByServiceTypeDtoDetail(ServiceType varchar(200),TotalSales decimal(18,2),Ordenes int)

	INSERT INTO #SalesByServiceTypeDtoDetail(ServiceType, TotalSales,Ordenes)
	SELECT
		M.cTipoServicio,
		ISNULL(I.nImporte, 0) AS Total,
		ISNULL(I.Cant, 0) AS Ordenes
	FROM #TiposServicio2 M
	LEFT JOIN #TiposServDetalle I ON M.nTipoServicio = I.nTipoServicio
	ORDER BY M.nTipoServicio;

	Select ServiceType, TotalSales FROM #SalesByServiceTypeDtoDetail

	DECLARE @nTotalTiposServicio int=(SELECT SUM(TotalSales) FROM #SalesByServiceTypeDtoDetail)

	Select ServiceType, TotalSales as sales, 
		CASE WHEN @nTotalTiposServicio=0 THEN 0 ELSE CONVERT(decimal(18,2),TotalSales/@nTotalTiposServicio)*100 END as porcentaje,
		Ordenes as ordenes,
		CONVERT(decimal(18,2),(TotalSales/@nTotalOrdenes)) as averageTicket,
		0.00 as trend,CONVERT(bit,1) as trendPositive
	FROM #SalesByServiceTypeDtoDetail

END

IF @nTipoDetalle=6
BEGIN
	-- Ventas por Estación de Cocina
	Create table #EstacionesCocina2 (nEstacionCocina int,cEstacionCocina varchar(200))

	Insert into #EstacionesCocina2
	Select nEstacionCocina,cDescripcion
	FROM CAT_EstacionesCocinas (NOLOCK)
	Where bActivo=1
	--select cEstacionCocina,count(1) as nCantidad from #DetalleVenta where cEstacionCocina='PIZZA' group by cEstacionCocina
	Select nEstacionCocina,cEstacionCocina,
	Cast (sum(nTotalConcepto + nServicioDomicilio ) as numeric(18,2)) as nImporte,
	COUNT(DISTINCT nConcepto) AS Cant
	into #Estaciones2
	From #DetalleVenta 
	Group by nEstacionCocina,cEstacionCocina
	Order by nEstacionCocina

	Create table #SalesByKitchenStationDetail(StationName varchar(200),TotalSales decimal(18,2),Productos int)

	INSERT INTO #SalesByKitchenStationDetail(StationName, TotalSales,Productos)
	SELECT
		M.cEstacionCocina,
		ISNULL(I.nImporte, 0) AS Total,
		ISNULL(I.Cant, 0) AS Cant
	FROM #EstacionesCocina2 M
	LEFT JOIN #Estaciones2 I ON M.nEstacionCocina = I.nEstacionCocina
	ORDER BY M.nEstacionCocina;

	Select StationName, TotalSales FROM #SalesByKitchenStationDetail

	DECLARE @nTotalEstacionCocina int=(SELECT SUM(TotalSales) FROM #SalesByKitchenStationDetail)

	Select StationName as station, TotalSales as sales,
	CASE WHEN @nTotalEstacionCocina=0 THEN 0 ELSE CONVERT(decimal(18,2),TotalSales/@nTotalEstacionCocina)*100 END as porcentaje,
		Productos as productos,
		CONVERT(decimal(18,2),(TotalSales/@nTotalOrdenes)) as averageTicket,
		0.00 as trend,CONVERT(bit,1) as trendPositive
	FROM #SalesByKitchenStationDetail
END
/*       
-- Tabla 0.- Concentrado de caja  
Select nTipo, nFormaPago, cFormaPago, sum(nImporte) as nImporte,  sum(nImporteUsuario ) as nImporteUsuario 
From #ConcentradoCaja   
Group by nTipo, nFormaPago, cFormaPago
Order by nTipo, nFormaPago, cFormaPago

-- Tabla 1.- Reporte de formas de pago  
Select --nIDCorteCaja, nIDApertura, 
nFormaPago,  cFormaPago, sum (nImporte) as nImporte, Cast((sum (nImporte)/@nTotalVenta) as numeric(18,6))  as nPorcentaje  
From #MovtosPagoOrden   
Group by nFormaPago, cFormaPago 
order by nImporte desc
-- Group by nIDCorteCaja, nIDApertura, 
--  
-- Tabla 2.- Reporte de tipos de servicios  
Select --nIDCorteCaja, nIDApertura, 
nTipoServicio,  cTipoServicio, sum (nImporte) as nImporte,  Cast((sum (nImporte)/@nTotalVenta) as numeric(18,6)) as nPorcentaje,
Count(distinct nOrden) as nCantOrden
From #MovtosPagoOrden   
Group by --nIDCorteCaja, nIDApertura, 
nTipoServicio, cTipoServicio 
Order by nImporte desc

-- Tabla 3.- Reporte de empleados con mas venta
Select top 10 * From #VentaEmpleados  
Order by nTotal desc  

-- Tabla 4.- Reporte de conceptos con mas venta 
Select top 10 P.* From (Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
(sum(nCantidad)* min(nImporteConcepto))  as nTotal
--sum(nImporteConcepto*nCantidad) as nTotal
From #DetalleVenta
Group by cConcepto) as P
Order by nCantidad desc 

-- Tabla 5.- Reporte de conceptos mas valiosos 
Select top 10 P.* From (Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
(sum(nCantidad)* min(nImporteConcepto))  as nTotal
--sum(nImporteConcepto*nCantidad) as nTotal
From #DetalleVenta
Group by cConcepto) as P
Order by nTotal desc
--select * from #DetalleVenta-- jam1
-- Tabla 6.- Reporte de ventas por cocina y detalle de estaciones de cocina 
--Select nCocina, cCocina, cEstacionCocina, sum(nTotalConcepto) as nImporte
Select cEstacionCocina, sum(nCantidad) as nCantidad, Cast (sum(nTotalConcepto + nServicioDomicilio ) as numeric(18,2)) as nImporte, 
Cast((sum(nTotalConcepto + nServicioDomicilio )/@nTotalSinServicio) as numeric(18,6))  as nPorcentaje  
From #DetalleVenta 
Group by cEstacionCocina
Order by nImporte desc

-- Tabla 7.- Órdenes Canceladas
--DECLARE @dFecha date=(SELECT dFecha FROM CAJ_RegistrosAperturaCaja (NOLOCK) WHERE nIDApertura=@IDApertura )
Select ISNULL(COUNT(1),0) as Cantidad, ISNULL(SUM(nTotal),0) as Total
from REG_OrdenesEncabezado Ord(NOLOCK)
Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
where 1=1 --(nIDApertura= @IDApertura OR (nIDApertura IS NULL AND dbo.FechaNumero_Fn(dFechaCancelacion)=dbo.FechaNumero_Fn(@dFecha)))
	and dbo.FechaNumero_Fn(dFechaCancelacion)=@vnFecha
	and Ord.nEstatus=6 -- 6=Cancelado

-- Tabla 8.- Cuentas Facturadas
Select ISNULL(COUNT(1),0) as Cantidad, ISNULL(SUM(CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END),0) as Total
from REG_OrdenesEncabezado Ord(NOLOCK)
join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
where 1=1 --nIDApertura= @IDApertura
	and nFactura IS NOT NULL and Ord.nEstatus<>6 and c.bActivo=1 AND isnull(C.bCancelado,0)=0

-- Tabla 9.- Comportamiento de Venta
----Select ISNULL(COUNT(1),0) as Cantidad, ISNULL(SUM(nTotal),0) as Total, CASE WHEN COUNT(1) IS NULL THEN 0 ELSE SUM(nTotal)/COUNT(1) END as Promedio
----from REG_OrdenesEncabezado Ord(NOLOCK)
----Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
----where 1=1 -- nIDApertura= @IDApertura
----	and Ord.nEstatus<>6 -- 6=Cancelado 
Select distinct @nTotalOrdenes as Cantidad, 
@nTotalVenta as Total, 
Cast(ISNULL(@nTotalVenta,0) / @nTotalOrdenes as numeric(18,2))as Promedio

-- Tabla 10.- Detalle diferencias por empleado

UPDATE CC SET
			  CC.nDiferencia= CASE WHEN @bTodo=1 THEN 
										isnull(CC.nDiferencia_Respaldo,CC.nDiferencia)									       
								  ELSE
										CC.nDiferencia
								  END
FROM #CAJ_DetalleCorteCaja CC 

Select E.nEmpleado, LTRIM(RTRIM(E.cNombre + ' ' + E.cApellidoPaterno + ' ' + ISNULL(E.cApellidoMaterno,''))) as cEmpleado,
CASE WHEN SUM(CASE WHEN nDiferencia<0 THEN nDiferencia*-1 ELSE 0 END)-SUM(CASE WHEN nDiferencia>0 THEN nDiferencia ELSE 0 END)>0 THEN
		  SUM(CASE WHEN nDiferencia<0 THEN nDiferencia*-1 ELSE 0 END)-SUM(CASE WHEN nDiferencia>0 THEN nDiferencia ELSE 0 END)
	 ELSE
		 0
	 END
as nSobrante,
CASE WHEN SUM(CASE WHEN nDiferencia<0 THEN nDiferencia*-1 ELSE 0 END)-SUM(CASE WHEN nDiferencia>0 THEN nDiferencia ELSE 0 END)<0 THEN
		  SUM(CASE WHEN nDiferencia<0 THEN nDiferencia*-1 ELSE 0 END)-SUM(CASE WHEN nDiferencia>0 THEN nDiferencia ELSE 0 END)
	 ELSE
		 0
	 END as nFaltante
From #CAJ_DetalleCorteCaja DCC
Join CAJ_CortesCaja CC (NOLOCK) ON DCC.nIDCorteCaja=CC.nIDCorteCaja
Join CAJ_RegistrosAperturaCaja AP (NOLOCK) ON CC.nIDApertura=AP.nIDApertura
Join CAT_Empleados E (NOLOCK) ON AP.nIDEmpleado=E.nEmpleado
Group by E.nEmpleado, LTRIM(RTRIM(E.cNombre + ' ' + E.cApellidoPaterno + ' ' + ISNULL(E.cApellidoMaterno,'')))
--select 'jam',* from  #CAJ_DetalleCorteCaja
-- Tabla 11.- Egresos 

SELECT ISNULL(CC.nConceptoCaja,0) as nConcepto,ISNULL(CC.cDescripcion,'RETIRO DE CAJA') as cConcepto,SUM(MC.nImporte) as nImporte
FROM CAJ_MovimientosCaja MC (NOLOCK)
JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
WHERE MC.bActivo=1 AND MC.nEfecto=-1
	--AND MC.nTipoRegistroCaja=2 -- Retiros de caja 
GROUP BY CC.nConceptoCaja,CC.cDescripcion

-- Tabla 12.- Ingresos

SELECT CC.nConceptoCaja as nConcepto,CC.cDescripcion as cConcepto,SUM(MC.nImporte) as nImporte
FROM CAJ_MovimientosCaja MC (NOLOCK)
JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
WHERE MC.bActivo=1 AND MC.nEfecto=1
AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
GROUP BY CC.nConceptoCaja,CC.cDescripcion
*/
End
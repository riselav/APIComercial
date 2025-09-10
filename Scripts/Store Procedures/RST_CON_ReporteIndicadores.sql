sp_eliminastore 'RST_CON_ReporteIndicadores'
GO
-- Select dbo.NumeroFecha_Fn(45865)
-- Select dbo.FechaNumero_Fn('20250731')
-- Exec RST_CON_ReporteIndicadores 1, 45839, 45869 , 0
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

-- ** INICIO DE DECLARACIONES CORREGIDAS **
Declare @vnFecha int,@vnFechaFinal int
-- Variables para el periodo previo
Declare @vnFechaInicialAnterior int
Declare @vnFechaFinalAnterior int
Declare @nTotalVentaPreviousPeriod numeric(18,2)
Declare @nTotalOrdenesPreviousPeriod int
Declare @nClientesPreviousPeriod int
Declare @nMesasOcupadasPreviousPeriod int
Declare @nTotalVentasFacturadasPreviousPeriod decimal(18,4)
Declare @nIngresosPreviousPeriod decimal(18,4)
Declare @nEgresosPreviousPeriod decimal(18,4)
Declare @nTicketPromedioPreviousPeriod decimal(18,2)
Declare @netIncomePreviousPeriod decimal(18,4)
Declare @occupancyRatePreviousPeriod decimal(18,4)
-- ** FIN DE DECLARACIONES CORREGIDAS **

Declare @dFechaInicialActual date = dbo.NumeroFecha_Fn(@FechaNumeroInicial)
Declare @dFechaFinalActual date = dbo.NumeroFecha_Fn(@FechaNumeroFinal)

Set @vnFechaInicialAnterior = dbo.FechaNumero_Fn(DATEADD(year, -1, @dFechaInicialActual))
Set @vnFechaFinalAnterior = dbo.FechaNumero_Fn(DATEADD(year, -1, @dFechaFinalActual))

SET @vnFecha=@FechaNumeroInicial
SET @vnFechaFinal=@FechaNumeroFinal

-- ** INICIO: CÁLCULO DE DATOS DEL PERIODO ACTUAL Y PREVIO FUERA DE LOS IF PARA ACCESO GLOBAL **

-- Cálculos para el PERIODO ACTUAL
Select C.* Into #CAJ_CortesCaja  
From CAJ_CortesCaja as c   
JOIN CAJ_RegistrosAperturaCaja AP (NOLOCK) ON C.nIDApertura=AP.nIDApertura
Where 1=1 
AND dbo.FechaNumero_Fn(AP.dFecha) BETWEEN @vnFecha AND @vnFechaFinal
    
Select c.* Into #CAJ_DetalleCorteCaja  
From CAJ_DetalleCorteCaja as c   
Join #CAJ_CortesCaja CC ON CC.nIDCorteCaja=c.nIDCorteCaja
  
SELECT AP.*
Into #CAJ_RegistrosAperturaCaja  
FROM CAJ_RegistrosAperturaCaja AP (NOLOCK)  
JOIN #CAJ_CortesCaja CC ON CC.nIDApertura=AP.nIDApertura

DECLARE @bTodo bit=~@bCr_Simul -- Neg.de @bCr_Simul

Select MC.* Into #CAJ_MovimientosCaja  
From CAJ_MovimientosCaja MC(Nolock)
Join #CAJ_CortesCaja CC ON CC.nIDCorteCaja=MC.nIDCorteCaja
Where MC.bActivo =1
AND ISNULL(MC.bRegistroEspecial,0)= CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END

Select MDC.* Into #CAJ_DetalleMovimientosCaja  
From CAJ_DetalleMovimientosCaja as MDC (Nolock)  
Inner Join #CAJ_MovimientosCaja as MC (Nolock) on MC.nIDRegistroCaja =mdc.nIDRegistroCaja 
Where MDC.bActivo =1

-- Obtiene solo los pagos de las ventas del periodo actual
SELECT MC.nTipoRegistroCaja, MC.nIDCorteCaja, MC.nIDApertura, 
OCE.nOrden, OCE.nCuenta, MDC.nFormaPago, FP.cDescripcion as cFormaPago, 
nImporte=   CASE WHEN @bTodo=1 THEN isnull(MDC.nImporte_Respaldo, MDC.nImporte) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE MDC.nImporte END END,
OE.nTipoServicio, trim(C.cDescripcion) as cTipoServicio, OE.nEmpleadoAbreMesa as nEmpleado, 
cEmpleado=Cast( E.cNombre + ' ' + isnull(cApellidoMaterno,'') + ' ' + isnull(cApellidoMaterno ,'') as Varchar(300)),  
nTotal=		CASE WHEN @bTodo=1 THEN isnull(MDC.nImporte_Respaldo, MDC.nImporte) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE OE.nTotal END END,
nDescuento= CASE WHEN @bTodo=1 THEN isnull(OE.nDescuento_Respaldo,OE.nDescuento) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE OE.nDescuento END END,
MC.bRegistroEspecial, 
AP.dFecha,OE.nCliente,OE.nMesa
Into #MovtosPagoOrden  
FROM #CAJ_MovimientosCaja As MC(NOLOCK)  
Inner Join CAJ_RegistrosAperturaCaja AP (NOLOCK) ON AP.nIDApertura=MC.nIDApertura
Inner Join #CAJ_DetalleMovimientosCaja as MDC (NOLOCK) on MDC.nIDRegistroCaja = MC.nIDRegistroCaja And MDC.bActivo =1 and MC.nTipoRegistroCaja = 5
Inner Join REG_OrdenesCuentasEncabezado as OCE (Nolock) on OCE.nOrden = MDC.nOrden and OCE.nCuenta =MDC.nCuenta and OCE.bActivo =1  
Inner Join CAT_FormasPago  as FP (Nolock) on FP.nFormaPago =MDC.nFormaPago   
Inner Join REG_OrdenesEncabezado  as OE (Nolock) on OE.norden = OCE.nOrden and OE.nEstatus IN(@nEstatusPagado,@nEstatusLiberado)
Left Join CAT_Empleados  as E (Nolock) on E.nEmpleado = OE.nEmpleadoAbreMesa   
Inner Join CAT_Catalogos as C (Nolock) on C.cNombre ='CAT_TipoServicio' and C.nCodigo = OE.nTipoServicio  
Where MC.bActivo =1 

Select
OE.nOrden, OE.nImporteServDom, OD.nConcepto, CV.cDescripcion as cConcepto, OD.nCantidad, OD.nImporte as nImporteConcepto, OD.nTotal as nTotalConcepto, OD.nEstacionCocina  as nEstacionCocina,
EC.cDescripcion  as cEstacionCocina, OD.nCocina, C.cDescripcion as cCocina, OD.nSubtotal, Cast(0 as numeric(18,4)) as nServicioDomicilio,
Cast(0 as numeric(18,2)) as nTotalCompleto, TCn.cDescripcion as cCategoria
Into #DetalleVenta
From REG_OrdenesEncabezado as OE (Nolock) 
Inner Join (Select nOrden From #MovtosPagoOrden Group by nOrden ) As P On P.nOrden =OE.nOrden 
Inner Join CAT_Empleados  as E (Nolock) on E.nEmpleado = OE.nEmpleadoAbreMesa   
Inner Join REG_OrdenesDetalle as OD (Nolock) on OD.nOrden = OE.nOrden  and OD.bActivo =1
	  And OD.bCancelado= CASE WHEN @bTodo=1 THEN OD.bCancelado ELSE 0 END
Inner Join CAT_ConceptosVenta  as CV (Nolock) on CV.nConceptoVenta = OD.nConcepto   
Inner Join CAT_TiposConceptos TCn (NOLOCK) ON TCn.nTipoConcepto=CV.nTipoConcepto
Inner Join CAT_EstacionesCocinas as EC (Nolock) on EC.nEstacionCocina = OD.nEstacionCocina 
left Join CAT_Cocinas  as C (Nolock) On C.nCocina =OD.nCocina 

Set @nTotalVenta = Isnull((Select sum(nImporte) as nImporte From #MovtosPagoOrden),0)
Set @nTotalOrdenes = Isnull((Select count(distinct norden) From #MovtosPagoOrden),0)

-- Cálculos para el PERIODO PREVIO
Select C.* Into #CAJ_CortesCaja_Prev From CAJ_CortesCaja as c JOIN CAJ_RegistrosAperturaCaja AP (NOLOCK) ON C.nIDApertura=AP.nIDApertura Where dbo.FechaNumero_Fn(AP.dFecha) BETWEEN @vnFechaInicialAnterior AND @vnFechaFinalAnterior
Select AP.* Into #CAJ_RegistrosAperturaCaja_Prev FROM CAJ_RegistrosAperturaCaja AP (NOLOCK) JOIN #CAJ_CortesCaja_Prev CC ON CC.nIDApertura=AP.nIDApertura
Select MC.* Into #CAJ_MovimientosCaja_Prev From CAJ_MovimientosCaja MC(Nolock) Join #CAJ_CortesCaja_Prev CC ON CC.nIDCorteCaja=MC.nIDCorteCaja Where MC.bActivo =1 AND ISNULL(MC.bRegistroEspecial,0)= CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
Select MDC.* Into #CAJ_DetalleMovimientosCaja_Prev From CAJ_DetalleMovimientosCaja as MDC (Nolock) Inner Join #CAJ_MovimientosCaja_Prev as MC (Nolock) on MC.nIDRegistroCaja =mdc.nIDRegistroCaja Where MDC.bActivo =1

SELECT MC.nTipoRegistroCaja, MC.nIDCorteCaja, MC.nIDApertura, OCE.nOrden, OCE.nCuenta, MDC.nFormaPago, FP.cDescripcion as cFormaPago, 
nImporte=CASE WHEN @bTodo=1 THEN isnull(MDC.nImporte_Respaldo, MDC.nImporte) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE MDC.nImporte END END,
OE.nTipoServicio, trim(C.cDescripcion) as cTipoServicio, OE.nEmpleadoAbreMesa as nEmpleado, 
cEmpleado=Cast( E.cNombre + ' ' + isnull(cApellidoMaterno,'') + ' ' + isnull(cApellidoMaterno ,'') as Varchar(300)),  
nTotal=CASE WHEN @bTodo=1 THEN isnull(MDC.nImporte_Respaldo, MDC.nImporte) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE OE.nTotal END END,
nDescuento= CASE WHEN @bTodo=1 THEN isnull(OE.nDescuento_Respaldo,OE.nDescuento) ELSE CASE WHEN MDC.bCancelado=1 THEN 0 ELSE OE.nDescuento END END,
MC.bRegistroEspecial, AP.dFecha,OE.nCliente,OE.nMesa
Into #MovtosPagoOrden_Prev
FROM #CAJ_MovimientosCaja_Prev As MC(NOLOCK)  
Inner Join CAJ_RegistrosAperturaCaja AP (NOLOCK) ON AP.nIDApertura=MC.nIDApertura
Inner Join #CAJ_DetalleMovimientosCaja_Prev as MDC (NOLOCK) on MDC.nIDRegistroCaja = MC.nIDRegistroCaja And MDC.bActivo =1 and MC.nTipoRegistroCaja = 5
Inner Join REG_OrdenesCuentasEncabezado as OCE (Nolock) on OCE.nOrden = MDC.nOrden and OCE.nCuenta =MDC.nCuenta and OCE.bActivo =1  
Inner Join CAT_FormasPago  as FP (Nolock) on FP.nFormaPago =MDC.nFormaPago   
Inner Join REG_OrdenesEncabezado  as OE (Nolock) on OE.norden = OCE.nOrden and OE.nEstatus IN(@nEstatusPagado,@nEstatusLiberado)
Left Join CAT_Empleados  as E (Nolock) on E.nEmpleado = OE.nEmpleadoAbreMesa   
Inner Join CAT_Catalogos as C (Nolock) on C.cNombre ='CAT_TipoServicio' and C.nCodigo = OE.nTipoServicio  
Where MC.bActivo =1

Set @nTotalVentaPreviousPeriod = Isnull((Select sum(nImporte) as nImporte From #MovtosPagoOrden_Prev),0)
Set @nTotalOrdenesPreviousPeriod = Isnull((Select count(distinct norden) From #MovtosPagoOrden_Prev),0)
Set @nClientesPreviousPeriod =(SELECT COUNT(DISTINCT nCliente) FROM #MovtosPagoOrden_Prev)
Set @nMesasOcupadasPreviousPeriod =(SELECT COUNT(DISTINCT nMesa) FROM #MovtosPagoOrden_Prev)

Set @nTotalVentasFacturadasPreviousPeriod =(
	Select ISNULL(SUM(CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END),0)
	from REG_OrdenesEncabezado Ord(NOLOCK)
	join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
	Join #CAJ_RegistrosAperturaCaja_Prev AP ON AP.nIDApertura=Ord.nIDApertura
	where nFactura IS NOT NULL and Ord.nEstatus<>6 and c.bActivo=1 AND isnull(C.bCancelado,0)=0
)

Set @nIngresosPreviousPeriod =(
	SELECT SUM(MC.nImporte)
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja_Prev AP ON AP.nIDApertura=MC.nIDApertura
	JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=1 AND MC.nTipoRegistroCaja=5
	AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
)
IF @nIngresosPreviousPeriod IS NULL SET @nIngresosPreviousPeriod=0

Set @nEgresosPreviousPeriod =(
	SELECT SUM(MC.nImporte)
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja_Prev AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=-1
)
IF @nEgresosPreviousPeriod IS NULL SET @nEgresosPreviousPeriod=0

Set @nTicketPromedioPreviousPeriod = (SELECT CASE WHEN @nTotalOrdenesPreviousPeriod=0 THEN 0.00 ELSE Cast(ISNULL(@nTotalVentaPreviousPeriod,0) / @nTotalOrdenesPreviousPeriod as numeric(18,2)) END)

DECLARE @nTotalMesas int=(SELECT COUNT(1) FROM CAT_Mesas (NOLOCK) WHERE bActivo=1 AND nSucursal=@nSucursal )
Set @occupancyRatePreviousPeriod = (CAST(@nMesasOcupadasPreviousPeriod AS DECIMAL(18,4)) / @nTotalMesas)

Set @netIncomePreviousPeriod =(@nTotalVentaPreviousPeriod-@nEgresosPreviousPeriod)

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

-- ** FIN: CÁLCULO DE DATOS DEL PERIODO ACTUAL Y PREVIO FUERA DE LOS IF PARA ACCESO GLOBAL **

IF @nTipoDetalle=0
BEGIN
	Select nEstacionCocina, norden
	Into #OrdenesCocina
	From #DetalleVenta
	
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

	Declare @nTotalSinServicio as numeric (18,2)=0
	Declare @nServDom as numeric (8,2)

	Set @nServDom= IsNull((Select sum(isnull(nImporteServDom,0)) From (Select nOrden, min(nImporteServDom) as nImporteServDom  From #DetalleVenta Group by nOrden) as SD),0)
	set @nTotalSinServicio= @nTotalVenta - @nServDom

	Select  nEmpleado,  cEmpleado, count(nOrden) as nTotalOrdenes, sum(nTotal) as nTotal,
	Case when @nTotalVenta =0 then 0 else Cast(sum(nTotal)/@nTotalVenta as numeric(18,2)) End as nPorcVentaProporcional
	Into #VentaEmpleados 
	From (Select nEmpleado, cEmpleado, nOrden, Sum(nTotal) as nTotal 
	From #MovtosPagoOrden Group by nEmpleado, cEmpleado, nOrden, cEmpleado)as MP
	Group by nEmpleado,  cEmpleado

	DECLARE @nTotalVentasFacturadas decimal(18,4)=(  
		Select 
			ISNULL(SUM(CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END),0) as Total
		from REG_OrdenesEncabezado Ord(NOLOCK)
		join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
		Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
		where 1=1
			and nFactura IS NOT NULL and Ord.nEstatus<>6 and c.bActivo=1 AND isnull(C.bCancelado,0)=0
	)
	
	DECLARE @nIngresos decimal(18,4)=(
		SELECT SUM(MC.nImporte) as nImporte
		FROM CAJ_MovimientosCaja MC (NOLOCK)
		JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
		JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
		WHERE MC.bActivo=1 AND MC.nEfecto=1 AND MC.nTipoRegistroCaja=5
		AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
	)

	IF @nIngresos IS NULL SET @nIngresos=0

	DECLARE @nEgresos decimal(18,4)=(
		SELECT SUM(MC.nImporte) as nImporte
		FROM CAJ_MovimientosCaja MC (NOLOCK)
		JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
		LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
		WHERE MC.bActivo=1 AND MC.nEfecto=-1
	)

	IF @nEgresos IS NULL SET @nEgresos=0

	DECLARE @nTicketPromedio decimal(18,2)= (SELECT CASE WHEN @nTotalOrdenes=0 THEN 0.00 ELSE Cast(ISNULL(@nTotalVenta,0) / @nTotalOrdenes as numeric(18,2)) END)

	DECLARE @occupancyRate decimal(18,4)

	SET @occupancyRate = (CAST(@nMesasOcupadas AS DECIMAL(18,4)) / @nTotalMesas)

	DECLARE @netIncome decimal(18,4) =(SELECT @nTotalVenta-@nEgresos)
	
	-- ** MODIFICACIÓN PARA INCLUIR VALORES DEL PERIODO PREVIO **
	SELECT totalSales=@nTotalVenta,
		   totalSalesPreviousPeriod=@nTotalVentaPreviousPeriod,
		   invoicedSales=@nTotalVentasFacturadas,
		   invoicedSalesPreviousPeriod=@nTotalVentasFacturadasPreviousPeriod,
		   uninvoicedSales=@nTotalVenta-@nTotalVentasFacturadas,
		   uninvoicedSalesPreviousPeriod=@nTotalVentaPreviousPeriod-@nTotalVentasFacturadasPreviousPeriod,
		   netIncome=@netIncome,
		   netIncomePreviousPeriod=@netIncomePreviousPeriod,
		   totalExpenses=@nEgresos,
		   totalExpensesPreviousPeriod=@nEgresosPreviousPeriod,
		   numberOfCustomers=@nClientes,
		   numberOfCustomersPreviousPeriod=@nClientesPreviousPeriod,
		   averageTicket=@nTicketPromedio,
		   averageTicketPreviousPeriod=@nTicketPromedioPreviousPeriod,
		   occupancyRate=@occupancyRate,
		   occupancyRatePreviousPeriod=@occupancyRatePreviousPeriod
	-- ** FIN DE MODIFICACIÓN **

	-- Ventas por Día
	SET DATEFIRST 1;
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

	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #Egresos
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=-1
	GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #Ingresos
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=1 AND MC.nTipoRegistroCaja=5
	AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END
	GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)
	
	INSERT INTO #IncomeVsExpenses (numMes, mes, income, expenses)
	SELECT
		M.numMes, M.mes, ISNULL(I.nImporte, 0) AS income, ISNULL(E.nImporte, 0) AS expenses
	FROM #MesesDelAño M
	LEFT JOIN #Ingresos I ON M.numMes = I.numMes
	LEFT JOIN #Egresos E ON M.numMes = E.numMes
	ORDER BY M.numMes;

	Select mes, income, expenses FROM #IncomeVsExpenses

	-- Venta Facturada vs No Facturada
	Create table #InvoicedVsUninvoiced(numMes int,mes varchar(100),invoiced decimal(18,2), uninvoiced decimal(18,2))

	Select MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes, 
	ISNULL(SUM(Case when ISNULL(nFactura,0)<>0 THEN CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END else 0 end),0) as Facturado,
	--ISNULL(SUM(Case when ISNULL(nFactura,0)=0 THEN C.nTotal else 0 end),0) as NoFacturado
	ISNULL(SUM(C.nTotal),0)-
	ISNULL(SUM(Case when ISNULL(nFactura,0)<>0 THEN CASE WHEN C.nImporteFactura>0 THEN C.nImporteFactura ELSE C.nTotal END else 0 end),0) as NoFacturado
	Into #FactVsNoFact
	from REG_OrdenesEncabezado Ord(NOLOCK)
	join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
	Join #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=Ord.nIDApertura
	where 1=1 and Ord.nEstatus<>6 and c.bActivo=1-- AND isnull(C.bCancelado,0)=0
	Group by MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)
	
	INSERT INTO #InvoicedVsUninvoiced (numMes, mes, invoiced, uninvoiced)
	SELECT M.numMes, M.mes, ISNULL(I.Facturado, 0) AS Facturado, ISNULL(I.NoFacturado, 0) AS NoFacturado
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
	SELECT M.nFormaPago, M.cFormaPago, ISNULL(I.nImporte, 0) AS Total
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
	Group by nTipoServicio, cTipoServicio 
	Order by nImporte desc

	Create table #SalesByServiceTypeDto(ServiceType varchar(200),TotalSales decimal(18,2))

	INSERT INTO #SalesByServiceTypeDto(ServiceType, TotalSales)
	SELECT M.cTipoServicio, ISNULL(I.nImporte, 0) AS Total
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
	SELECT M.cEstacionCocina, ISNULL(I.nImporte, 0) AS Total
	FROM #EstacionesCocina M
	LEFT JOIN #Estaciones I ON M.nEstacionCocina = I.nEstacionCocina
	ORDER BY M.nEstacionCocina;

	Select StationName, TotalSales FROM #SalesByKitchenStationDto

	-- Reporte de conceptos con más venta 
	Select top 10 P.cConcepto as DishName,P.nCantidad as Quantity 
	From (Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, (sum(nCantidad)* min(nImporteConcepto))  as nTotal
	From #DetalleVenta
	Group by cConcepto) as P
	Order by nCantidad desc 

	-- Reporte de conceptos más valiosos 
	Select top 10 P.cConcepto as DishName,P.nTotal as RevenueOrProfit 
	From (Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, (sum(nCantidad)* min(nImporteConcepto))  as nTotal
	From #DetalleVenta
	Group by cConcepto) as P
	Order by nTotal desc
End

IF @nTipoDetalle=1
BEGIN
	-- ** INICIO DE MODIFICACIONES para Detalle de Ventas por Categoría **
	
	-- Calculo de ventas por categoria para el periodo previo
	Select P.cCategoria as name, P.nTotalConcepto as previousSales
	Into #DetalleVenta_Prev
	From (
		Select TCn.cDescripcion as cCategoria, sum(OD.nTotal) as nTotalConcepto
		From REG_OrdenesEncabezado as OE (Nolock) 
		Inner Join (Select nOrden From #MovtosPagoOrden_Prev Group by nOrden ) As P On P.nOrden =OE.nOrden 
		Inner Join REG_OrdenesDetalle as OD (Nolock) on OD.nOrden = OE.nOrden  and OD.bActivo =1
		Inner Join CAT_ConceptosVenta  as CV (Nolock) on CV.nConceptoVenta = OD.nConcepto   
		Inner Join CAT_TiposConceptos TCn (NOLOCK) ON TCn.nTipoConcepto=CV.nTipoConcepto
		Group by TCn.cDescripcion) as P
	
	Select P.cCategoria as name,P.nTotalConcepto as valor
	From (Select cCategoria, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
		  (sum(nCantidad)* min(nImporteConcepto)) as nTotal,
		  (sum(nTotalConcepto)) as nTotalConcepto
		   From #DetalleVenta
		   Group by cCategoria) as P
	Order by P.cCategoria
	
	Select P.cCategoria as category,
	 P.nTotalConcepto as sales, 
	CONVERT(decimal(18,2),CASE WHEN @nTotalVenta = 0 THEN 0 ELSE (P.nTotalConcepto/@nTotalVenta)*100 END) as porcentaje,
	 P.products,
	CONVERT(decimal(18,2),CASE WHEN @nTotalOrdenes = 0 THEN 0 ELSE (P.nTotalConcepto/@nTotalOrdenes) END) as averageTicket,
	CONVERT(decimal(18,2),CASE WHEN ISNULL(P_Prev.previousSales, 0) = 0 THEN 100 ELSE ((P.nTotalConcepto - ISNULL(P_Prev.previousSales,0)) / ISNULL(P_Prev.previousSales,0)) * 100 END) as trend,
	CONVERT(bit,CASE WHEN P.nTotalConcepto >= ISNULL(P_Prev.previousSales,0) THEN 1 ELSE 0 END) as trendPositive,
	ISNULL(P_Prev.previousSales,0) as previousSales
	From (Select cCategoria, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, 
		  (sum(nCantidad)* min(nImporteConcepto)) as nTotal,
		  (sum(nTotalConcepto)) as nTotalConcepto,
		   count(nConcepto) as products
		   From #DetalleVenta
		   Group by cCategoria) as P
	LEFT JOIN #DetalleVenta_Prev AS P_Prev ON P.cCategoria = P_Prev.name
	Order by P.cCategoria
	
	-- ** FIN DE MODIFICACIONES para Detalle de Ventas por Categoría **
END

IF @nTipoDetalle=2
BEGIN
	-- ** INICIO DE MODIFICACIONES para Ingresos Vs Gastos **
	Create table #IncomeVsExpensesDetail(numMes int,mes varchar(100),income decimal(18,2), expenses decimal(18,2),gananciaNeta decimal(18,2))
	Create table #IncomeVsExpensesDetail_Prev(numMes int,mes varchar(100),income decimal(18,2), expenses decimal(18,2),gananciaNeta decimal(18,2))

	SET LANGUAGE Spanish;

	-- ** Egresos
	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #EgresosDetalle
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=-1 GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	-- ** Egresos Previos
	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #EgresosDetalle_Prev
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja_Prev AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=-1 GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)
	
	-- ** Ingresos
	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #IngresosDetalle
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=1 AND MC.nTipoRegistroCaja=5
		AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)
	
	-- ** Ingresos Previos
	SELECT MONTH(AP.dFecha) as numMes,DATENAME(MONTH,AP.dFecha) as cMes,SUM(MC.nImporte) as nImporte
	Into #IngresosDetalle_Prev
	FROM CAJ_MovimientosCaja MC (NOLOCK)
	JOIN #CAJ_RegistrosAperturaCaja_Prev AP ON AP.nIDApertura=MC.nIDApertura
	LEFT JOIN CAT_ConceptosCaja CC (NOLOCK) ON CC.nConceptoCaja=MC.nConceptoCaja
	WHERE MC.bActivo=1 AND MC.nEfecto=1 AND MC.nTipoRegistroCaja=5
		AND ISNULL(MC.bRegistroEspecial,0)=CASE WHEN @bTodo=1 THEN 0 ELSE ISNULL(MC.bRegistroEspecial,0) END GROUP BY MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	INSERT INTO #IncomeVsExpensesDetail (numMes, mes, income, expenses,gananciaNeta)
	SELECT
		M.numMes, M.mes, ISNULL(I.nImporte, 0) AS income, ISNULL(E.nImporte, 0) AS expenses, ISNULL(I.nImporte, 0)-ISNULL(E.nImporte, 0)
	FROM #MesesDelAño M
	LEFT JOIN #IngresosDetalle I ON M.numMes = I.numMes
	LEFT JOIN #EgresosDetalle E ON M.numMes = E.numMes
	ORDER BY M.numMes;

	INSERT INTO #IncomeVsExpensesDetail_Prev (numMes, mes, income, expenses,gananciaNeta)
	SELECT
		M.numMes, M.mes, ISNULL(I.nImporte, 0) AS income, ISNULL(E.nImporte, 0) AS expenses, ISNULL(I.nImporte, 0)-ISNULL(E.nImporte, 0)
	FROM #MesesDelAño M
	LEFT JOIN #IngresosDetalle_Prev I ON M.numMes = I.numMes
	LEFT JOIN #EgresosDetalle_Prev E ON M.numMes = E.numMes
	ORDER BY M.numMes;

	Select mes, income, expenses,gananciaNeta FROM #IncomeVsExpensesDetail
	
	Select
		d.mes,
		d.income,
		d.expenses,
		d.gananciaNeta as netProfit,
		Convert(decimal(18,2),Case When d.income=0 THEN 0 ELSE (d.gananciaNeta/d.income)*100 END) as margin,
		CONVERT(decimal(18,2),CASE WHEN ISNULL(d_prev.gananciaNeta, 0) = 0 THEN 100 ELSE ((d.gananciaNeta - ISNULL(d_prev.gananciaNeta,0)) / ISNULL(d_prev.gananciaNeta,0)) * 100 END) as trend,
		CONVERT(bit,CASE WHEN d.gananciaNeta >= ISNULL(d_prev.gananciaNeta,0) THEN 1 ELSE 0 END) as trendPositive,
		ISNULL(d_prev.gananciaNeta, 0) as previousNetProfit
	FROM #IncomeVsExpensesDetail d
	LEFT JOIN #IncomeVsExpensesDetail_Prev d_prev ON d.mes = d_prev.mes
	
	-- ** FIN DE MODIFICACIONES para Ingresos Vs Gastos **
END

IF @nTipoDetalle=3
BEGIN
	-- ** INICIO DE MODIFICACIONES para Venta Facturada vs No Facturada **
	Create table #InvoicedVsUninvoicedDetail(numMes int,mes varchar(100),ventasFacturadas decimal(18,2), ventasNoFacturadas decimal(18,2),ventaTotal decimal(18,2))
	Create table #InvoicedVsUninvoicedDetail_Prev(numMes int,mes varchar(100),ventasFacturadas decimal(18,2), ventasNoFacturadas decimal(18,2),ventaTotal decimal(18,2))

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
		and Ord.nEstatus<>6 and c.bActivo=1-- AND isnull(C.bCancelado,0)=0
	Group by MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	-- Periodo previo
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
	Into #FactVsNoFactDetalle_Prev
	from REG_OrdenesEncabezado Ord(NOLOCK)
	join REG_OrdenesCuentasEncabezado C (NOLOCK) ON Ord.nOrden=C.nOrden
	Join #CAJ_RegistrosAperturaCaja_Prev AP ON AP.nIDApertura=Ord.nIDApertura
	where 1=1
		and Ord.nEstatus<>6 and c.bActivo=1 AND isnull(C.bCancelado,0)=0
	Group by MONTH(AP.dFecha),DATENAME(MONTH,AP.dFecha)

	INSERT INTO #InvoicedVsUninvoicedDetail (numMes, mes, ventasFacturadas, ventasNoFacturadas,ventaTotal)
	SELECT M.numMes, M.mes, ISNULL(I.Facturado, 0) AS Facturado, ISNULL(I.NoFacturado, 0) AS NoFacturado, ISNULL(I.nVentaTotal, 0) AS nVentaTotal
	FROM #MesesDelAño M LEFT JOIN #FactVsNoFactDetalle I ON M.numMes = I.numMes ORDER BY M.numMes;
	
	INSERT INTO #InvoicedVsUninvoicedDetail_Prev (numMes, mes, ventasFacturadas, ventasNoFacturadas,ventaTotal)
	SELECT M.numMes, M.mes, ISNULL(I.Facturado, 0) AS Facturado, ISNULL(I.NoFacturado, 0) AS NoFacturado, ISNULL(I.nVentaTotal, 0) AS nVentaTotal
	FROM #MesesDelAño M LEFT JOIN #FactVsNoFactDetalle_Prev I ON M.numMes = I.numMes ORDER BY M.numMes;

	Select d.mes, d.ventasFacturadas, d.ventasNoFacturadas
	FROM #InvoicedVsUninvoicedDetail d

	Select d.mes,d.ventaTotal,
	d.ventasFacturadas, Case When d.ventaTotal =0 Then 0 Else Convert(decimal(18,2), (d.ventasFacturadas/d.ventaTotal)*100) End as invoicedPercentage,
	d.ventasNoFacturadas,
	d.ventasFacturadas+d.ventasNoFacturadas as Total,Case When d.ventaTotal =0 Then 0 Else 100-Convert(decimal(18,2), (d.ventasFacturadas/d.ventaTotal)*100) End as uninvoicedPercentage,
	CONVERT(decimal(18,2),CASE WHEN ISNULL(d_prev.ventaTotal, 0) = 0 THEN 100 ELSE ((d.ventaTotal - ISNULL(d_prev.ventaTotal,0)) / ISNULL(d_prev.ventaTotal,0)) * 100 END) as trend,
	CONVERT(bit,CASE WHEN d.ventaTotal >= ISNULL(d_prev.ventaTotal,0) THEN 1 ELSE 0 END) as trendPositive,
	ISNULL(d_prev.ventasFacturadas, 0) as previousInvoiced,
	ISNULL(d_prev.ventasNoFacturadas, 0) as previousUninvoiced,
	ISNULL(d_prev.ventaTotal, 0) as previousTotal
	FROM #InvoicedVsUninvoicedDetail d
	LEFT JOIN #InvoicedVsUninvoicedDetail_Prev d_prev ON d.mes = d_prev.mes
	
	-- ** FIN DE MODIFICACIONES para Venta Facturada vs No Facturada **
END

IF @nTipoDetalle=4
BEGIN
	-- ** INICIO DE MODIFICACIONES para Ventas por Forma de Pago **
	Create table #SalesByPaymentMethodDetail(nFormaPago int,PaymentMethod varchar(200),TotalSales decimal(18,2),Cant int)
	Create table #SalesByPaymentMethodDetail_Prev(nFormaPago int,PaymentMethod varchar(200),TotalSales decimal(18,2),Cant int)

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
	
	Select  
	nFormaPago, cFormaPago, sum (nImporte) as nImporte,COUNT(1) as nCant
	into #FormasPagoMovtosDetalle_Prev
	From #MovtosPagoOrden_Prev
	Group by nFormaPago, cFormaPago 
	order by nImporte desc
	
	INSERT INTO #SalesByPaymentMethodDetail(nFormaPago, PaymentMethod, TotalSales,Cant)
	SELECT M.nFormaPago, M.cFormaPago, ISNULL(I.nImporte, 0) AS Total, ISNULL(I.nCant, 0) AS Cant
	FROM #CAT_FormasPago2 M LEFT JOIN #FormasPagoMovtosDetalle I ON M.nFormaPago = I.nFormaPago ORDER BY M.nFormaPago;

	INSERT INTO #SalesByPaymentMethodDetail_Prev(nFormaPago, PaymentMethod, TotalSales,Cant)
	SELECT M.nFormaPago, M.cFormaPago, ISNULL(I.nImporte, 0) AS Total, ISNULL(I.nCant, 0) AS Cant
	FROM #CAT_FormasPago2 M LEFT JOIN #FormasPagoMovtosDetalle_Prev I ON M.nFormaPago = I.nFormaPago ORDER BY M.nFormaPago;

	Select PaymentMethod, TotalSales FROM #SalesByPaymentMethodDetail

	DECLARE @nTotalFormasPago int=(SELECT SUM(TotalSales) FROM #SalesByPaymentMethodDetail)

	Select d.PaymentMethod, d.TotalSales as sales, 
		CASE WHEN @nTotalFormasPago=0 THEN 0 ELSE CONVERT(decimal(18,2),d.TotalSales/@nTotalFormasPago)*100 END as porcentaje,
		d.cant as transactions,
		CASE WHEN @nTotalOrdenes =0 THEN 0 ELSE CONVERT(decimal(18,2),(d.TotalSales/@nTotalOrdenes)) END as averageTicket,
		CONVERT(decimal(18,2),CASE WHEN ISNULL(d_prev.TotalSales, 0) = 0 THEN 100 ELSE ((d.TotalSales - ISNULL(d_prev.TotalSales,0)) / ISNULL(d_prev.TotalSales,0)) * 100 END) as trend,
		CONVERT(bit,CASE WHEN d.TotalSales >= ISNULL(d_prev.TotalSales,0) THEN 1 ELSE 0 END) as trendPositive,
		ISNULL(d_prev.TotalSales, 0) as previousTotalSales
	FROM #SalesByPaymentMethodDetail d
	LEFT JOIN #SalesByPaymentMethodDetail_Prev d_prev ON d.nFormaPago = d_prev.nFormaPago
	
	-- ** FIN DE MODIFICACIONES para Ventas por Forma de Pago **
END

IF @nTipoDetalle=5
BEGIN
	-- ** INICIO DE MODIFICACIONES para Ventas por Tipo de Servicio **
	Create table #TiposServicio2 (nTipoServicio int,cTipoServicio varchar(200))
	Create table #SalesByServiceTypeDtoDetail(ServiceType varchar(200),TotalSales decimal(18,2),Ordenes int)
	Create table #SalesByServiceTypeDtoDetail_Prev(ServiceType varchar(200),TotalSales decimal(18,2),Ordenes int)

	Insert into #TiposServicio2
	Select nCodigo,LTRIM(RTRIM(cDescripcion)) FROM CAT_Catalogos (NOLOCK) WHere cNombre= 'CAT_TipoServicio'
	
	Select 
	nTipoServicio, cTipoServicio, sum (nImporte) as nImporte,COUNT(1) as Cant
	into #TiposServDetalle
	From #MovtosPagoOrden   
	Group by nTipoServicio, cTipoServicio 
	Order by nImporte desc
	
	Select 
	nTipoServicio, cTipoServicio, sum (nImporte) as nImporte,COUNT(1) as Cant
	into #TiposServDetalle_Prev
	From #MovtosPagoOrden_Prev
	Group by nTipoServicio, cTipoServicio 
	Order by nImporte desc

	INSERT INTO #SalesByServiceTypeDtoDetail(ServiceType, TotalSales,Ordenes)
	SELECT M.cTipoServicio, ISNULL(I.nImporte, 0) AS Total, ISNULL(I.Cant, 0) AS Ordenes
	FROM #TiposServicio2 M LEFT JOIN #TiposServDetalle I ON M.nTipoServicio = I.nTipoServicio ORDER BY M.nTipoServicio;
	
	INSERT INTO #SalesByServiceTypeDtoDetail_Prev(ServiceType, TotalSales,Ordenes)
	SELECT M.cTipoServicio, ISNULL(I.nImporte, 0) AS Total, ISNULL(I.Cant, 0) AS Ordenes
	FROM #TiposServicio2 M LEFT JOIN #TiposServDetalle_Prev I ON M.nTipoServicio = I.nTipoServicio ORDER BY M.nTipoServicio;

	Select ServiceType, TotalSales FROM #SalesByServiceTypeDtoDetail

	DECLARE @nTotalTiposServicio int=(SELECT SUM(TotalSales) FROM #SalesByServiceTypeDtoDetail)

	Select d.ServiceType, d.TotalSales as sales, 
		CASE WHEN @nTotalTiposServicio=0 THEN 0 ELSE CONVERT(decimal(18,2),d.TotalSales/@nTotalTiposServicio)*100 END as porcentaje,
		d.Ordenes as ordenes,
		CASE WHEN @nTotalOrdenes=0 THEN 0 ELSE CONVERT(decimal(18,2),(d.TotalSales/@nTotalOrdenes)) END as averageTicket,
		CONVERT(decimal(18,2),CASE WHEN ISNULL(d_prev.TotalSales,0) = 0 THEN 100 ELSE ((d.TotalSales - ISNULL(d_prev.TotalSales,0)) / ISNULL(d_prev.TotalSales,0)) * 100 END) as trend,
		CONVERT(bit,CASE WHEN d.TotalSales >= ISNULL(d_prev.TotalSales,0) THEN 1 ELSE 0 END) as trendPositive,
		ISNULL(d_prev.TotalSales, 0) as previousTotalSales
	FROM #SalesByServiceTypeDtoDetail d
	LEFT JOIN #SalesByServiceTypeDtoDetail_Prev d_prev ON d.ServiceType = d_prev.ServiceType

	-- ** FIN DE MODIFICACIONES para Ventas por Tipo de Servicio **
END

IF @nTipoDetalle=6
BEGIN
	-- ** INICIO DE MODIFICACIONES para Ventas por Estación de Cocina **
	Create table #EstacionesCocina2 (nEstacionCocina int,cEstacionCocina varchar(200))
	Create table #SalesByKitchenStationDetail(StationName varchar(200),TotalSales decimal(18,2),Productos int)
	Create table #SalesByKitchenStationDetail_Prev(StationName varchar(200),TotalSales decimal(18,2),Productos int)

	Insert into #EstacionesCocina2
	Select nEstacionCocina,cDescripcion
	FROM CAT_EstacionesCocinas (NOLOCK)
	Where bActivo=1
	
	Select nEstacionCocina,cEstacionCocina,
	Cast (sum(nTotalConcepto + nServicioDomicilio ) as numeric(18,2)) as nImporte,
	--COUNT(DISTINCT nConcepto) AS Cant
	SUM(nCantidad) AS Cant
	into #Estaciones2
	From #DetalleVenta 
	Group by nEstacionCocina,cEstacionCocina
	Order by nEstacionCocina
	
	Select EC.nEstacionCocina, EC.cDescripcion as cEstacionCocina,
	Cast (sum(OD.nTotal) as numeric(18,2)) as nImporte,
	COUNT(DISTINCT OD.nConcepto) AS Cant
	into #Estaciones2_Prev
	From REG_OrdenesEncabezado as OE (Nolock) 
	Inner Join (Select nOrden From #MovtosPagoOrden_Prev Group by nOrden ) As P On P.nOrden =OE.nOrden 
	Inner Join REG_OrdenesDetalle as OD (Nolock) on OD.nOrden = OE.nOrden  and OD.bActivo =1
	Inner Join CAT_EstacionesCocinas as EC (Nolock) on EC.nEstacionCocina = OD.nEstacionCocina 
	Group by EC.nEstacionCocina, EC.cDescripcion
	Order by EC.nEstacionCocina

	INSERT INTO #SalesByKitchenStationDetail(StationName, TotalSales,Productos)
	SELECT M.cEstacionCocina, ISNULL(I.nImporte, 0) AS Total, ISNULL(I.Cant, 0) AS Cant
	FROM #EstacionesCocina2 M LEFT JOIN #Estaciones2 I ON M.nEstacionCocina = I.nEstacionCocina ORDER BY M.nEstacionCocina;
	
	INSERT INTO #SalesByKitchenStationDetail_Prev(StationName, TotalSales,Productos)
	SELECT M.cEstacionCocina, ISNULL(I.nImporte, 0) AS Total, ISNULL(I.Cant, 0) AS Cant
	FROM #EstacionesCocina2 M LEFT JOIN #Estaciones2_Prev I ON M.nEstacionCocina = I.nEstacionCocina ORDER BY M.nEstacionCocina;

	Select StationName, TotalSales FROM #SalesByKitchenStationDetail

	DECLARE @nTotalEstacionCocina int=(SELECT SUM(TotalSales) FROM #SalesByKitchenStationDetail)

	Select d.StationName as station, d.TotalSales as sales,
	CASE WHEN @nTotalEstacionCocina=0 THEN 0 ELSE CONVERT(decimal(18,2),d.TotalSales/@nTotalEstacionCocina)*100 END as porcentaje,
		d.Productos as productos,
	CASE WHEN @nTotalOrdenes=0 THEN 0 ELSE CONVERT(decimal(18,2),(d.TotalSales/@nTotalOrdenes)) END as averageTicket,
	CONVERT(decimal(18,2),CASE WHEN ISNULL(d_prev.TotalSales,0) = 0 THEN 100 ELSE ((d.TotalSales - ISNULL(d_prev.TotalSales,0)) / ISNULL(d_prev.TotalSales,0)) * 100 END) as trend,
	CONVERT(bit,CASE WHEN d.TotalSales >= ISNULL(d_prev.TotalSales,0) THEN 1 ELSE 0 END) as trendPositive,
	ISNULL(d_prev.TotalSales, 0) as previousTotalSales
	FROM #SalesByKitchenStationDetail d
	LEFT JOIN #SalesByKitchenStationDetail_Prev d_prev ON d.StationName = d_prev.StationName
	
	-- ** FIN DE MODIFICACIONES para Ventas por Estación de Cocina **
END

IF @nTipoDetalle=7
BEGIN
	-- ** INICIO DE MODIFICACIONES para Reporte de conceptos con más venta **
	
	-- Conceptos del periodo previo
	Select cConcepto, sum(nCantidad) as nCantidad_Prev
	into #ConceptosVenta_Prev
	From (
		Select OD.nConcepto, CV.cDescripcion as cConcepto, OD.nCantidad
		From REG_OrdenesEncabezado as OE (Nolock) 
		Inner Join (Select nOrden From #MovtosPagoOrden_Prev Group by nOrden ) As P On P.nOrden =OE.nOrden 
		Inner Join REG_OrdenesDetalle as OD (Nolock) on OD.nOrden = OE.nOrden  and OD.bActivo =1
		Inner Join CAT_ConceptosVenta  as CV (Nolock) on CV.nConceptoVenta = OD.nConcepto   
	) as P
	Group by cConcepto
	
	Select top 10 P.cConcepto as DishName,P.nCantidad as Quantity 
	From (Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, (sum(nCantidad)* min(nImporteConcepto))  as nTotal
	From #DetalleVenta
	Group by cConcepto) as P
	Order by nCantidad desc 

	Select top 10 ROW_NUMBER()OVER(Order by P.nCantidad desc) as ranking ,P.cConcepto as DishName,P.cCategoria,P.nCantidad as Quantity,P.nPrecio,P.nTotal, 
	CONVERT(decimal(18,2),CASE WHEN ISNULL(P_Prev.nCantidad_Prev, 0) = 0 THEN 100 ELSE ((P.nCantidad - ISNULL(P_Prev.nCantidad_Prev,0)) / ISNULL(P_Prev.nCantidad_Prev,0)) * 100 END) as trend,
	CONVERT(bit,CASE WHEN P.nCantidad >= ISNULL(P_Prev.nCantidad_Prev,0) THEN 1 ELSE 0 END) as trendPositive,
	ISNULL(P_Prev.nCantidad_Prev, 0) as previousQuantity
	From 
	(Select cConcepto,cCategoria, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, (sum(nCantidad)* min(nImporteConcepto))  as nTotal
	From #DetalleVenta
	Group by cConcepto,cCategoria) as P
	LEFT JOIN #ConceptosVenta_Prev P_Prev ON P.cConcepto = P_Prev.cConcepto
	Order by nCantidad desc
	
	-- ** FIN DE MODIFICACIONES para Reporte de conceptos con más venta **
END

IF @nTipoDetalle=8
BEGIN
	-- ** INICIO DE MODIFICACIONES para Reporte de conceptos más valiosos **
	
	-- Conceptos del periodo previo
	Select cConcepto, sum(nTotal) as nTotal_Prev
	into #ConceptosValiosos_Prev
	From (
		Select OD.nConcepto, CV.cDescripcion as cConcepto, OD.nTotal as nTotal
		From REG_OrdenesEncabezado as OE (Nolock) 
		Inner Join (Select nOrden From #MovtosPagoOrden_Prev Group by nOrden ) As P On P.nOrden =OE.nOrden 
		Inner Join REG_OrdenesDetalle as OD (Nolock) on OD.nOrden = OE.nOrden  and OD.bActivo =1
		Inner Join CAT_ConceptosVenta  as CV (Nolock) on CV.nConceptoVenta = OD.nConcepto   
	) as P
	Group by cConcepto
	
	Select top 10 P.cConcepto as DishName,P.nTotal as TotalSales 
	From (Select cConcepto, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, (sum(nCantidad)* min(nImporteConcepto))  as nTotal
	From #DetalleVenta
	Group by cConcepto) as P
	Order by nTotal desc

	Select top 10 ROW_NUMBER()OVER(Order by P.nTotal desc) as ranking,P.cConcepto as DishName,P.cCategoria,
	P.nTotal as TotalSales,
	0.00 as Costo,
	P.nTotal - 0.00 as ganancia,
	CONVERT(decimal(18,2), ((P.nTotal - 0.00) / P.nTotal) * 100) AS Margen,
	CONVERT(decimal(18,2),CASE WHEN ISNULL(P_Prev.nTotal_Prev,0) = 0 THEN 100 ELSE ((P.nTotal - ISNULL(P_Prev.nTotal_Prev,0)) / ISNULL(P_Prev.nTotal_Prev,0)) * 100 END) as trend,
	CONVERT(bit,CASE WHEN P.nTotal >= ISNULL(P_Prev.nTotal_Prev,0) THEN 1 ELSE 0 END) as trendPositive,
	ISNULL(P_Prev.nTotal_Prev, 0) as previousTotalSales
	From 
	(Select cConcepto,cCategoria, sum(nCantidad) as nCantidad, min(nImporteConcepto) as nPrecio, (sum(nCantidad)* min(nImporteConcepto))  as nTotal
	From #DetalleVenta
	Group by cConcepto,cCategoria) as P
	LEFT JOIN #ConceptosValiosos_Prev P_Prev ON P.cConcepto = P_Prev.cConcepto
	Order by nTotal desc
	
	-- ** FIN DE MODIFICACIONES para Reporte de conceptos más valiosos **
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
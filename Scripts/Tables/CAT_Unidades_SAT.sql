-- drop table [CAT_Unidades_SAT]
CREATE TABLE [CAT_Unidades_SAT]
(
	nId int identity(1,1) primary key,
	cTipo varchar(100) NOT NULL,    -- id de proveedor
	[cClave] varchar(10) NOT NULL,    -- Clave SAT
	[cNombre] varchar(50) NULL,    -- nombre de la clave SAT	
	bActivo bit default 1,
)
GO



--select * from CAT_Unidades_SAT

INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Múltiplos / Fracciones / Decimales','H87','Pieza')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de venta','EA','Elemento')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades específicas de la industria (varias)','E48','Unidad de servicio')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de venta','ACT','Actividad')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Mecánica','KGM','Kilogramo')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades específicas de la industria (varias)','E51','Trabajo')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Diversos','A9','Tarifa')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Tiempo y Espacio','MTR','Metro')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Diversos','AB','Paquete a granel')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades especificas de la industria (varias)','BB','Caja base')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de venta','KT','KIT')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de venta','SET','Conjunto')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Tiempo y Espacio','LTR','Litro')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de empaque','XBX','Caja')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Tiempo y Espacio','MON','Mes')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Tiempo y Espacio','HUR','Hora')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Tiempo y Espacio','MTK','Metro Cuadrado')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Diversos','11','Equipos')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Mecánica','MGM','Miligramo')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de empaque','XPK','Paquete')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de empaque','XKI','Kit (Conjunto de piezas)')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Diversos','AS','Variedad')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Mecánica','GRM','Gramo')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Números en enteros / Números / Ratios','PR','Par')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de venta','DPC','Docenas de piezas')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de empaque','XUN','Unidad')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Tiempo y Espacio','DAY','Día')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades de empaque','XLT','Lote')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Diversos','10','Grupos')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Tiempo y Espacio','MLT','Mililitro')
GO
INSERT INTO CAT_Unidades_SAT(cTipo,cClave,cNombre) 
VALUES ('Unidades específicas de la industria (varias)','E54','Viaje')
GO
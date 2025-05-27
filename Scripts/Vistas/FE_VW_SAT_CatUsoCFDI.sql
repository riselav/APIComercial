sp_eliminavista 'FE_VW_SAT_CatUsoCFDI'
go
Create view FE_VW_SAT_CatUsoCFDI As   
SELECT UsoCFDI, Descripcion FROM SAT_CatUsoCFDI (NOLOCK)
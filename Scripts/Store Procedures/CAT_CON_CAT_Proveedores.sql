sp_eliminastore 'CAT_CON_CAT_Proveedores'

GO
-- Exec CAT_CON_CAT_Proveedores 1
CREATE procedure CAT_CON_CAT_Proveedores (  
@nProveedor as int
)  
As   
Begin  

	Set NoCount On

Select nProveedor, cDescripcionComercial, cDescripcionFiscal, cColonia, cCodigoPostal, 
       cRFC, cCURP, nTipoPersona, bNacional,  nDiasCredito, cTelefono, cCorreo,bActivo
From CAT_Proveedores (Nolock)
Where ( nProveedor=@nProveedor or @nProveedor=0) 

  
End
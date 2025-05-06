namespace Voalaft.Data.Entidades
{
    public class CatProveedores
    {
        public int Folio { get; set; }

        public string? DescripcionComercial { get; set; }

        public string? DescripcionFiscal { get; set; }

        public string? cColonia { get; set; }

        public string? cCodigoPostal { get; set; }

        public string? cRFC { get; set; }

        public string? cCURP{ get; set; }

        public int nTipoPersona {   get; set;}

        public Boolean  bNacional { get; set; }

        public  int nDiasCredito { get; set; }  

        public string? cTelefono { get; set; }

        public string? cCorreo { get; set; }

        public Boolean bActivo { get; set; }

        public string? cUsuario { get; set; }

        public string? cMaquina { get; set; }

        public List<CatContactoProveedor> lstContactosProveedores { get; set; } = new();

    }
}



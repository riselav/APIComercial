using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.Tableros
{
    public class Cliente
    {
        public long codigoCliente { get; set; }  // EM.nCliente
        public string? nombreComercial { get; set; }  // EM.cNombreCompleto
        public string? calle { get; set; }  // EM.cCalle
        public string? colonia { get; set; }  // CL.cNombreAsentamiento
        public string? ciudad { get; set; }  // LC.cDescripcion
        public string? estado { get; set; }  // Est.cNombreEstado
        public string? regimenFiscal { get; set; }  // RF.cDescripcion
        public string? razonSocial { get; set; }  // CR.cRazonSocial
        public string? rfc { get; set; }  // CR.cRFC
        public bool activo { get; set; }  // EM.bActivo
    }
}

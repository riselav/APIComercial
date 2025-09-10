using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades.ClasesParametros
{
    public class ParamCancelaVenta
    {
        public long? nVenta { get; set; }
        [Required(ErrorMessage = "El motivo de cancelación es obligatorio.")]
        public int motivo { get; set; }
        [StringLength(200, ErrorMessage = "La observación no puede exceder los 200 caracteres.")]
        public string? observacion { get; set; }
        public string? usuarioAutorizo { get; set; }
        public string? usuarioCancelo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}

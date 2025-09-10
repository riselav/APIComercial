using System.ComponentModel.DataAnnotations;

namespace Voalaft.Data.Entidades
{
    public class CatMotivos
    {
        public int nMotivo { get; set; }
        public string cDescripcion { get; set; }
        public int nTipoMotivo { get; set; }
        public bool bActivo { get; set; }
        public string? cMaquina_Registra { get; set; }
        public string? cUsuario_Registra { get; set; }
        public DateTime? dFecha_Registra { get; set; }
        public string? cMaquina_Modifica { get; set; }
        public string? cUsuario_Modifica { get; set; }
        public DateTime? dFecha_Modifica { get; set; }
        public string? cMaquina_Cancela { get; set; }
        public string? cUsuario_Cancela { get; set; }
        public DateTime? dFecha_Cancela { get; set; }
    }
}

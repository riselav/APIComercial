using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voalaft.Data.Entidades
{
    public class CatDenominacion
    {
        public int Denominacion { get; set; }
        public string? Descripcion { get; set; }
        public int Tipo { get; set; }
        public string? cImagen { get; set; }  // Podrías cambiar a byte[] si usas imágenes en binario/base64

        public decimal? Valor { get; set; }

        public bool Activo { get; set; }
        public string? Usuario { get; set; }
        public string? Maquina { get; set; }
    }
}
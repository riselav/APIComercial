//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Voalaft.Utilerias
//{
//    public class ImpuestosSAT
//    {
//        public string c_impuesto { get; set; }
//        public string Descripcion { get; set; }
//        public bool EsRetencion { get; set; }
//        public bool EsTraslado { get; set; }

//        public List<ImpuestosSAT> ObtenerListaImpuestosSAT()
//        {
//            List<ImpuestosSAT> resultado = new List<ImpuestosSAT>();
//            resultado.Add(
//                new ImpuestosSAT()
//                {
//                    c_impuesto = "001",
//                    Descripcion="ISR",
//                    EsRetencion=true, 
//                    EsTraslado=false
//                }
//                );
//            resultado.Add(
//                new ImpuestosSAT()
//                {
//                    c_impuesto = "002",
//                    Descripcion = "IVA",
//                    EsRetencion = true,
//                    EsTraslado = true
//                }
//                );
//            resultado.Add(
//                new ImpuestosSAT()
//                {
//                    c_impuesto = "003",
//                    Descripcion = "IEPS",
//                    EsRetencion = true,
//                    EsTraslado = true
//                }
//                );
//            return resultado;
//        }

//        public ImpuestosSAT ObtenerImpuestoSATPorC_Impuesto(string c_impuesto)
//        {
//            return ObtenerListaImpuestosSAT().FirstOrDefault(impuesto => impuesto.c_impuesto == c_impuesto);
//        }

//        public string ObtenerDescripcionImpuestoSATPorC_Impuesto(string c_impuesto)
//        {
//            ImpuestosSAT imp= ObtenerListaImpuestosSAT().FirstOrDefault(impuesto => impuesto.c_impuesto == c_impuesto);
//            if (imp != null)
//            {
//                return imp.Descripcion;
//            }
//            else
//            {
//                return string.Empty;
//            }

//        }
//    }
//}

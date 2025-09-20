using System;
using System.Collections.Generic;
using System.IO; // Removed System.Drawing  
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp; // Added SkiaSharp  
using BarcodeStandard;
using Type = BarcodeStandard.Type;

namespace Voalaft.Utilerias
{
    public class BarcodeGenerator
    {
        public static string GenerateBarcodeBase64(string text)
        {
            try
            {
                // Crea una instancia de la clase Barcode  
                BarcodeStandard.Barcode barcode = new BarcodeStandard.Barcode();

                // Define las propiedades del código de barras  
                barcode.IncludeLabel = true; // Incluye el texto debajo del código de barras  
                barcode.Alignment = AlignmentPositions.Center;

                // Genera el código de barras como una imagen  
                var barcodeImage = barcode.Encode(Type.Code128, text.Trim(), new SKColorF(0, 0, 0), new SKColorF(1, 1, 1), 600, 100);

                // Convierte la imagen a un flujo de memoria  
                using (MemoryStream ms = new MemoryStream())
                {
                    // Guarda la imagen en el flujo en formato PNG  
                    barcodeImage.Encode().SaveTo(ms);

                    // Convierte el arreglo de bytes de la imagen a una cadena Base64  
                    byte[] imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);

                    return base64String;
                }

            }
            catch (Exception)
            {
                
                return "";
            }
        }
    }
}

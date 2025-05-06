using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Unicode;
using System.Text;
using System;
using Newtonsoft.Json;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using System.Text.RegularExpressions;

namespace Voalaft.API.Utils
{
    class PrimitiveToStringConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(decimal) || objectType == typeof(decimal?);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
    public class CryptographyUtils
    {

        private static readonly string llavesecreta = ConfigurationSingleton.Instance.GetSection("JWT:key").Value; //"79B134EF79F30F177AA8A2E9147F18BA483B44ED";
        private const int KeySize = 16; 

        public static T DeserializarPeticion<T>(string mensajeDesencriptado)
        {
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Serialize };

            T peticion = JsonConvert.DeserializeObject<T>(mensajeDesencriptado, serializerSettings);

            return peticion;
        }


        public static string SerializarObjeto(object objetoDeserializado)
        {
            var serializerSettings = new JsonSerializerSettings { PreserveReferencesHandling = PreserveReferencesHandling.Objects, ReferenceLoopHandling = ReferenceLoopHandling.Serialize, DateTimeZoneHandling = DateTimeZoneHandling.Unspecified, DateFormatString = "yyyy-MM-ddTHH:mm:ss", Converters = { new PrimitiveToStringConverter() } };

            string mensajeSerializado = JsonConvert.SerializeObject(objetoDeserializado, serializerSettings);

            return mensajeSerializado;
        }

        public static string Desencriptar(string ciphertext)
        {
            var result = "";
            try
            {
                var cipheredBytes = Convert.FromBase64String(ciphertext);
                var keyBytes = Encoding.UTF8.GetBytes(llavesecreta).Take(KeySize).ToArray();

                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.None; 
                    aes.Key = keyBytes;
                    aes.IV = keyBytes; 

                    using (var decryptor = aes.CreateDecryptor())
                    {                  
                        var paddedData = ApplyPadding(cipheredBytes, KeySize);

                        var decryptedData = decryptor.TransformFinalBlock(paddedData, 0, paddedData.Length);
                        result = Encoding.UTF8.GetString(decryptedData);
                    }
                }
                result = new string(result.Where(c => char.IsLetterOrDigit(c) || char.IsPunctuation(c) || char.IsWhiteSpace(c)).ToArray());
                int endIndex = result.LastIndexOf('}'); 
                result = result.Substring(0, endIndex + 1);

                result = BalancearLlaves(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return null;
            }
            return result;
        }
        private static string BalancearLlaves(string json)
        {
            // Contar las llaves abiertas
            int llavesAbiertas = json.Count(c => c == '{');

            // Agregar llaves cerradas si es necesario
            while (json.Count(c => c == '{') > json.Count(c => c == '}'))
            {
                json += "}";
            }
            int llavesCerradas = json.Count(c => c == '}');

            while(llavesAbiertas < llavesCerradas)
            {
                int endI = json.LastIndexOf('}'); 
                json = json.Remove(endI, 1);
                llavesAbiertas = json.Count(c => c == '{');
                llavesCerradas = json.Count(c => c == '}');
            }
            int endIndex = json.LastIndexOf('}'); // Suponiendo que el JSON termina con }
            json = json.Substring(0, endIndex + 1);

            return json;
        }
        public static byte[] ApplyPadding(byte[] data, int blockSize)
        {
            int paddingBytes = blockSize - (data.Length % blockSize);
            byte[] paddedData = new byte[data.Length + paddingBytes];
            Buffer.BlockCopy(data, 0, paddedData, 0, data.Length);

            for (int i = 0; i < paddingBytes; i++)
            {
                paddedData[data.Length + i] = (byte)(0x20 << (24 - (data.Length + i) % 4 * 8));
            }

            return paddedData;
        }

        
        public static byte[] TruncateKey(byte[] key, int size)
        {
            return key.Take(size).ToArray();
        }
        public static byte[] GetKeyBytes(string key)
        {
            return Encoding.UTF8.GetBytes(key).Take(KeySize).ToArray();
        }

        
        public static string EncryptForce(string plainText, byte[] key, byte[] iv)
        {
            try
            {
                using (var aes = new AesCryptoServiceProvider())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.Key = TruncateKey(key, KeySize);
                    aes.IV = iv;

                    using (var encryptor = aes.CreateEncryptor())
                    {
                        var plainBytes = Encoding.UTF8.GetBytes(plainText);
                        var paddedData = ApplyPadding(plainBytes, KeySize);
                        var encryptdData = encryptor.TransformFinalBlock(paddedData, 0, paddedData.Length);
                        return Convert.ToBase64String(encryptdData);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex}");
                return null;
            }
        }

        public static string Encrypt( string plainText)
        {
            try
            {
                var keyBytes = TruncateKey(GetKeyBytes(llavesecreta),KeySize);
                var plainBytes = Encoding.UTF8.GetBytes(plainText);

                var cipherBytes = EncryptForce(plainText, keyBytes, keyBytes);
                return cipherBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: " + ex.Message);
                return null;
            }
        }

       
        static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static ResultadoAPI CrearResultado(object respuesta)
        {
            try
            {
                ResultadoAPI resultado = new ResultadoAPI();
                string respuestaString = SerializarObjeto(respuesta);
                string cadenaEncriptada = Encrypt(respuestaString);
                resultado.contenido = cadenaEncriptada;
                resultado.hash = EncriptarSha256(respuestaString);
                return resultado;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: " + ex.Message);
                return null;
            }
        }
        public static String EncriptarSha256(String valor)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                Byte[] result = hash.ComputeHash(enc.GetBytes(valor));

                foreach (Byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }

        public static string EncryptDB(string input)
        {
            try
            {
                return GenerateEncryptedDB(input);
            }
            catch (Exception ex)
            {
                return string.Empty; // Or handle the error differently
            }
        }

        private static string GenerateEncryptedDB(string input)
        {
            var key = ConfigurationSingleton.Instance.GetSection("KeyDB").Value;
            var key64b = ConfigurationSingleton.Instance.GetSection("KeyDB64b").Value;
            byte[] IV = Encoding.ASCII.GetBytes(key); // Assuming 'key' is a string
            byte[] EncryptionKey = Convert.FromBase64String(key64b); // Assuming 'key64b' is a string
            byte[] buffer = Encoding.UTF8.GetBytes(input);

            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Key = EncryptionKey;
                des.IV = IV;

                return Convert.ToBase64String(des.CreateEncryptor().TransformFinalBlock(buffer,
         0, buffer.Length));
            }
        }

        public static string DecryptDB(string
         input)
        {
            try
            {
                return GenerateDecryptedDB(input);
            }
            catch (Exception ex)
            {   
                return string.Empty; // Or handle the error differently
            }
        }

        private static string GenerateDecryptedDB(string input)
        {
            var key = ConfigurationSingleton.Instance.GetSection("KeyDB").Value;
            var key64b = ConfigurationSingleton.Instance.GetSection("KeyDB64b").Value;
            byte[] IV = Encoding.ASCII.GetBytes(key); // Assuming 'key' is a string
            byte[] EncryptionKey = Convert.FromBase64String(key64b); // Assuming 'key64b' is a string
            byte[] buffer = Convert.FromBase64String(input);

            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Key = EncryptionKey;
                des.IV = IV;

                return Encoding.UTF8.GetString(des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));

            }

        }

    }
}
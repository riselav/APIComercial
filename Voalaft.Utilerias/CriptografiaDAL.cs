using System;
using System.Security.Cryptography;
using System.Text;

namespace SQLConnector
{
    public class CriptografiaDAL
    {
        private readonly string clave = "12345678";
        private readonly string clave64b = "AMQt0O0zHADQL8oCaTFVKnEsac3FMRSW";

        public string Encriptar(string entrada)
        {
            string resultado = "";
            try
            {
                resultado = GenerarEncriptado(entrada);
            }
            catch (Exception ex)
            {
                
            }

            return resultado;
        }

        private string GenerarEncriptado(string input)
        {
            byte[] IV = Encoding.ASCII.GetBytes(clave); // La clave debe ser de 8 caracteres
            byte[] encryptionKey = Convert.FromBase64String(clave64b); // No se puede alterar la cantidad de caracteres, pero sí la clave
            byte[] buffer = Encoding.UTF8.GetBytes(input);

            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Key = encryptionKey;
                des.IV = IV;

                byte[] result = des.CreateEncryptor().TransformFinalBlock(buffer, 0, buffer.Length);
                return Convert.ToBase64String(result);
            }
        }

        public string Desencriptar(string entrada)
        {
            string resultado = "";
            try
            {
                resultado = GenerarDesencriptado(entrada);
            }
            catch (Exception ex)
            {
                
            }

            return resultado;
        }

        private string GenerarDesencriptado(string entrada)
        {
            byte[] IV = Encoding.ASCII.GetBytes(clave); // 8 caracteres
            byte[] encryptionKey = Convert.FromBase64String(clave64b);
            byte[] buffer = Convert.FromBase64String(entrada);

            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                des.Key = encryptionKey;
                des.IV = IV;

                byte[] decrypted = des.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(decrypted);
            }
        }

        public string EncriptarContraseña(string contraseña)
        {
            try
            {
                const int clave = 1314282103;
                StringBuilder resultado = new StringBuilder();

                for (int i = 0; i < contraseña.Length; i++)
                {
                    int valor = (int)contraseña[i] + clave;

                    if (i > 0)
                        resultado.Append(";");

                    resultado.Append(valor);
                }

                return resultado.Length > 0 ? resultado.ToString() : clave.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al encriptar: " + ex.Message);
                return null;
            }
        }


        public string DesencriptarContraseña(string entrada)
        {
            try
            {
                const int clave = 1314282103;
                string[] partes = entrada.Split(';');
                StringBuilder resultado = new StringBuilder();

                foreach (string parte in partes)
                {
                    if (int.TryParse(parte, out int valor))
                    {
                        int ascii = valor - clave;
                        resultado.Append((char)ascii);
                    }
                    else
                    {
                        // Valor inválido en la cadena
                        return null;
                    }
                }

                return resultado.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al desencriptar: " + ex.Message);
                return null;
            }
        }


        public string ObtenerSHA256(string texto)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(texto);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

    }
}
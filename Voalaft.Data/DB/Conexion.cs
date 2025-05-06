using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
namespace Voalaft.Data.DB
{
    public class Conexion
    {
        private readonly IConfiguration _configuracion;
        private readonly string _cadenaSql;

        public Conexion(IConfiguration configuration)
        {
            _configuracion = configuration;
            _cadenaSql = _configuracion.GetConnectionString("voalaftConection")!;
        }

        public SqlConnection ObtenerSqlConexion()
        {
            var builder = new SqlConnectionStringBuilder(_cadenaSql);
            builder.TrustServerCertificate = true;
            return new SqlConnection(builder.ConnectionString);
        }

        // Método para obtener el esquema de la tabla
        public DataTable ObtenerEsquemaTabla(string tableName)
        {
            DataTable schemaTable = new DataTable();
            using (SqlConnection con = new SqlConnection(_cadenaSql))
            {
                con.Open();
                using (SqlDataAdapter adapter = new SqlDataAdapter($"SELECT * FROM {tableName} WHERE 1 = 0", con))
                {
                    adapter.FillSchema(schemaTable, SchemaType.Source);
                }
            }
            return schemaTable;
        }

               public Boolean InsertarConBulkCopy(SqlConnection con, string tableName, DataTable dataTable)
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dataTable);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false; // Hubo un error
            }

        }

    }
}

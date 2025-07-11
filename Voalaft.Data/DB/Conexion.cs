using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
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
                return false;
            }
        }

        public Boolean InsertarConBulkCopy(SqlConnection con, string tableName, DataTable dataTable, SqlTransaction trans )
        {
            try
            {
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans))
                {
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dataTable);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        // Para volúmenes pequeños(<1000 registros) : Usa la Opción 2 más simple
        public Boolean UpsertConBulkCopySimple(SqlConnection con, string tableName, DataTable dataTable, SqlTransaction trans, string[] keyColumns)
        {
            try
            {
                // 1. Separar registros existentes de nuevos
                DataTable existingRecords = new DataTable();
                DataTable newRecords = new DataTable();

                // Copiar estructura
                existingRecords = dataTable.Clone();
                newRecords = dataTable.Clone();

                // 2. Verificar cuáles registros ya existen
                foreach (DataRow row in dataTable.Rows)
                {
                    string whereClause = string.Join(" AND ", keyColumns.Select(col => $"{col} = '{row[col]}'"));
                    string existsQuery = $"SELECT COUNT(*) FROM {tableName} WHERE {whereClause}";

                    using (SqlCommand cmd = new SqlCommand(existsQuery, con, trans))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            existingRecords.ImportRow(row);
                        }
                        else
                        {
                            newRecords.ImportRow(row);
                        }
                    }
                }

                // 3. Insertar registros nuevos con BulkCopy
                if (newRecords.Rows.Count > 0)
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con, SqlBulkCopyOptions.Default, trans))
                    {
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(newRecords);
                    }
                }

                // 4. Actualizar registros existentes uno por uno
                foreach (DataRow row in existingRecords.Rows)
                {
                    string whereClause = string.Join(" AND ", keyColumns.Select(col => $"{col} = @{col}"));
                    string setClause = string.Join(", ", dataTable.Columns.Cast<DataColumn>()
                        .Where(col => !keyColumns.Contains(col.ColumnName))
                        .Select(col => $"{col.ColumnName} = @{col.ColumnName}"));

                    string updateQuery = $"UPDATE {tableName} SET {setClause} WHERE {whereClause}";

                    using (SqlCommand cmd = new SqlCommand(updateQuery, con, trans))
                    {
                        foreach (DataColumn column in dataTable.Columns)
                        {
                            cmd.Parameters.AddWithValue($"@{column.ColumnName}", row[column.ColumnName] ?? DBNull.Value);
                        }
                        cmd.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en UpsertConBulkCopySimple: {ex.Message}");
                return false;
            }
        }

    }
}

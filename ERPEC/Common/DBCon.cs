using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ERPEC.Common
{
    public class DBCon
    {

        public static DataTable GetData(SqlConnection connection, SqlTransaction transaction, string Query, Dictionary<string, object> SQLparams)
        {
            DataTable data_table = new DataTable();

            using (SqlCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                if (transaction != null)
                    command.Transaction = transaction;
                command.CommandText = Query;

                if (SQLparams != null)
                {
                    SQLparams.ToList().ForEach(p =>
                    {
                        command.Parameters.AddWithValue(p.Key, p.Value);
                    });
                }
                using (SqlDataAdapter sql_tadpt = new SqlDataAdapter(command))
                {
                    sql_tadpt.Fill(data_table);
                }

                command.ExecuteNonQuery();
            }

            return data_table;
        }

        public static void ExecuteNonQuery(SqlConnection connection, SqlTransaction transaction, string Query, Dictionary<string, object> SQLparams)
        {
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                if (transaction != null)
                    command.Transaction = transaction;
                command.CommandText = Query;
                if (SQLparams != null)
                {
                    SQLparams.ToList().ForEach(p =>
                    {
                        command.Parameters.AddWithValue(p.Key, p.Value);
                    });
                }
                command.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(SqlConnection connection, SqlTransaction transaction, string Query, Dictionary<string, object> SQLparams)
        {
            object scalar_val = null;
            using (SqlCommand command = connection.CreateCommand())
            {
                command.Connection = connection;
                if (transaction != null)
                    command.Transaction = transaction;
                command.CommandText = Query;
                if (SQLparams != null)
                {
                    SQLparams.ToList().ForEach(p =>
                    {
                        command.Parameters.AddWithValue(p.Key, p.Value);
                    });
                }
                scalar_val = command.ExecuteScalar();
            }
            return scalar_val;
        }
    
    }
}
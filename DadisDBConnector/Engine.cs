using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
 

namespace DadisDBConnector
{
    public class Engine
    {
        public Engine(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public string ConnectionString { get; set; }


        public DataTable Query(string query)
        {
            DataTable resultado = new DataTable();
            

            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);

                MySqlDataReader reader = command.ExecuteReader();

                DataTable table = new DataTable("Query");
                resultado.Load(reader);  
                reader.Close();
            }
            
                return resultado;
        }



    }
}

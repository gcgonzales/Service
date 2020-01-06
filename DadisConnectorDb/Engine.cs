using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using MySql.Data.MySqlClient;
 

namespace DadisConnectorDb
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
                connection.Open();
                MySqlCommand command = new MySqlCommand(query.ToLower(), connection);
                MySqlDataReader reader = command.ExecuteReader();

                DataTable table = new DataTable("Query");
                resultado.Load(reader);  
                reader.Close();

                connection.Close();
            }
            
                return resultado;
        }

        public int Execute(string instruction)
        {

            int resultado = 0;
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(instruction.ToLower(), connection);
                resultado = command.ExecuteNonQuery();
                connection.Close();
            }

            return resultado;
        }



    }
}

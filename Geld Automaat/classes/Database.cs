using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Geld_Automaat.classes
{
    public class myDBconnection
    {
        public MySqlConnection connection;

        public myDBconnection()
        {
            // Define the connection string
            string connectionString = "Server=localhost;Uid=root;Pwd=;Database=mydb";

            // Initialize the MySqlConnection object
            connection = new MySqlConnection(connectionString);
        }

        // Method to open the database connection
        public bool Connect()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    return true; // Connection successful
                }
                else
                {
                    return false; // Connection is already open
                }
            }
            catch (Exception ex)
            {
                // Handle connection errors
                Console.WriteLine("Error: " + ex.Message);
                return false; // Connection failed
            }
        }

        // Method to close the database connection
        public void CloseConnection()
        {
            if (connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }
    }
}
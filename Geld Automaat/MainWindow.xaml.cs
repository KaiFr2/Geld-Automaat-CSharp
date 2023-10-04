using Geld_Automaat.classes;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Geld_Automaat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string enteredRekeningnummer;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_num1(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("1");
        }

        private void Button_Click_num2(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("2");
        }

        private void Button_Click_num3(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("3");
        }

        private void Button_Click_num4(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("4");
        }

        private void Button_Click_num5(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("5");
        }

        private void Button_Click_num6(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("6");
        }

        private void Button_Click_num7(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("7");
        }

        private void Button_Click_num8(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("8");
        }

        private void Button_Click_num9(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("9");
        }

        private void Button_Click_num0(object sender, RoutedEventArgs e)
        {
            AppendNumberToTextBox("0");
        }

        private void AppendNumberToTextBox(string number)
        {
            if (PincodeTextBox.Text.Length < 4)
            {
                PincodeTextBox.Text += number;
            }
        }


        //Hash ding 
        public static string EncryptString(string rawData)
        {  // Create a SHA256
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //Button enter
        private void Button_Click_enter(object sender, RoutedEventArgs e)
        {
            string enteredRekeningnummer = RekeningnummerTextBox.Text;
            string enteredPincode = PincodeTextBox.Text; // Leave the pincode as is

            // Hash the pincode with SHA-256
            string hashedPincode = EncryptString(enteredPincode);

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT Rekeningnummer, saldo, Pincode FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";

                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the encrypted rekeningnummer and stored pincode from the database
                            string storedRekeningnummer = reader["Rekeningnummer"].ToString();
                            string storedPincode = reader["Pincode"].ToString();

                            if (enteredRekeningnummer == storedRekeningnummer && hashedPincode == storedPincode)
                            {
                                int saldo = Convert.ToInt32(reader["saldo"]);

                                MessageBox.Show("Login Successful. Welcome to the ATM!");

                                homescreen.Visibility = Visibility.Hidden;
                                options.Visibility = Visibility.Visible;

                                // Display a message without showing the original rekeningnummer
                                WelcomeLabel.Content = "Welcome to the ATM!";
                                SaldoLabel.Content = "Je saldo is " + saldo;
                            }
                            else
                            {
                                MessageBox.Show("Invalid Rekeningnummer or PIN. Please try again.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid Rekeningnummer or PIN. Please try again.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }




        //Delete knop
        private void Button_Click_delete(object sender, RoutedEventArgs e)
        {
            if (PincodeTextBox.Text.Length > 0)
            {
                PincodeTextBox.Text = PincodeTextBox.Text.Substring(0, PincodeTextBox.Text.Length - 1);

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        //Admin login button
        private void Button_Admin_login(object sender, RoutedEventArgs e)
        {
            string enteredUsername = gebruikernaam.Text;
            string enteredPassword = password.Password;

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT * FROM medewerkers WHERE gebruikersnaam = @gebruikersnaam AND wachtwoord = @wachtwoord";
                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@gebruikersnaam", enteredUsername);
                    command.Parameters.AddWithValue("@wachtwoord", enteredPassword);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show("Admin Login Successful. Welcome!");
                            admindjalla.Visibility = Visibility.Hidden;
                            admindjalla2.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            MessageBox.Show("Invalid admin username or password. Please try again.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        //Storten pagina
        private void Button_Storten(object sender, RoutedEventArgs e)
        {
            options.Visibility = Visibility.Hidden;
            storten.Visibility = Visibility.Visible;

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT saldo FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";

                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                    // ExecuteScalar is used to retrieve a single value (saldo) from the database
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        // Convert the result to an integer
                        int saldo = Convert.ToInt32(result);

                        Saldo3.Content = "Je saldo is: " + saldo;
                    }
                    else
                    {
                        MessageBox.Show("Rekeningnummer not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        private void Button_Opnemenknop(object sender, RoutedEventArgs e)
        {
            options.Visibility = Visibility.Hidden;
            Opnemen.Visibility = Visibility.Visible;

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT saldo FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";

                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                    // ExecuteScalar is used to retrieve a single value (saldo) from the database
                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        // Convert the result to an integer
                        int saldo = Convert.ToInt32(result);

                        Saldo2.Content = "Je saldo is: " + saldo;
                    }
                    else
                    {
                        MessageBox.Show("Rekeningnummer not found.");
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            admindjalla.Visibility = Visibility.Visible;
            homescreen.Visibility = Visibility.Hidden;
        }


        //100 euro storten button
        // Define a class-level variable to keep track of the deposit count
        private int OpnemenCount = 0;

        private void Button_StortGeld100(object sender, RoutedEventArgs e)
        {
            // Check if the user has already made three deposits

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` + 100 WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";

                            MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                            updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            updateSaldoCommand.ExecuteNonQuery();

                            string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 2, 100)";
                            MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            insertTransactionCommand.ExecuteNonQuery();

                            transaction.Commit();

                            MessageBox.Show("Deposit successful. 100 euros added to your account.");

                            UpdateSaldoPlus(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine("Error: " + ex.Message);
                            MessageBox.Show("An error occurred during the deposit. Please try again later.");
                        }
                    }
                }
                else
                {
                    // Connection to the database failed
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., database errors
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        //200 euro storten button
        private void Button_StortGeld200(object sender, RoutedEventArgs e)
        {
            // Check if the user has already made three deposits

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` + 200 WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";

                            MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                            updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            updateSaldoCommand.ExecuteNonQuery();

                            string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 2, 200)";
                            MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            insertTransactionCommand.ExecuteNonQuery();

                            transaction.Commit();

                            MessageBox.Show("Deposit successful. 200 euros added to your account.");

                            UpdateSaldoPlus(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine("Error: " + ex.Message);
                            MessageBox.Show("An error occurred during the deposit. Please try again later.");
                        }
                    }
                }
                else
                {
                    // Connection to the database failed
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., database errors
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        //500 euro storten button
        private void Button_StortGeld500(object sender, RoutedEventArgs e)
        {
            // Check if the user has already made three deposits

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` + 500 WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";

                            MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                            updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            updateSaldoCommand.ExecuteNonQuery();

                            string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 2, 500)";
                            MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            insertTransactionCommand.ExecuteNonQuery();

                            transaction.Commit();

                            MessageBox.Show("Deposit successful. 500 euros added to your account.");

                            UpdateSaldoPlus(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine("Error: " + ex.Message);
                            MessageBox.Show("An error occurred during the deposit. Please try again later.");
                        }
                    }
                }
                else
                {
                    // Connection to the database failed
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., database errors
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        private void Button_Opnemen100(object sender, RoutedEventArgs e)
        {
            // Check if the user has already made three deposits
            if (OpnemenCount >= 3)
            {
                MessageBox.Show("You have reached the maximum amount of withdrawing for the day (3 times).");
                return;
            }

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` - 100 WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";

                            MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                            updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            updateSaldoCommand.ExecuteNonQuery();

                            string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 1, 100)";
                            MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            insertTransactionCommand.ExecuteNonQuery();

                            OpnemenCount++;

                            transaction.Commit();
                            MessageBox.Show("Deposit successful. 100 euros added to your account.");

                            UpdateSaldoMin(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine("Error: " + ex.Message);
                            MessageBox.Show("An error occurred during the deposit. Please try again later.");
                        }
                    }
                }
                else
                {
                    // Connection to the database failed
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., database errors
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        //200 euro storten button
        private void Button_Opnemen200(object sender, RoutedEventArgs e)
        {
            // Check if the user has already made three deposits
            if (OpnemenCount >= 3)
            {
                MessageBox.Show("You have reached the maximum amount of withdrawing for the day (3 times).");
                return;
            }

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` - 200 WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";

                            MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                            updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            updateSaldoCommand.ExecuteNonQuery();

                            string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 1, 200)";
                            MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            insertTransactionCommand.ExecuteNonQuery();

                            OpnemenCount++;

                            transaction.Commit();
                            MessageBox.Show("Deposit successful. 100 euros added to your account.");

                            UpdateSaldoMin(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine("Error: " + ex.Message);
                            MessageBox.Show("An error occurred during the deposit. Please try again later.");
                        }
                    }
                }
                else
                {
                    // Connection to the database failed
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., database errors
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        //500 euro storten button
        private void Button_Opnemen500(object sender, RoutedEventArgs e)
        {
            // Check if the user has already made three deposits
            if (OpnemenCount >= 3)
            {
                MessageBox.Show("You have reached the maximum amount of withdrawing for the day (3 times).");
                return;
            }

            string enteredRekeningnummer = RekeningnummerTextBox.Text; // Leave the rekeningnummer as is

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` - 500 WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";

                            MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                            updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            updateSaldoCommand.ExecuteNonQuery();

                            string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 1, 500)";
                            MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            insertTransactionCommand.ExecuteNonQuery();

                            OpnemenCount++;

                            transaction.Commit();
                            MessageBox.Show("Deposit successful. 500 euros added to your account.");

                            UpdateSaldoMin(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine("Error: " + ex.Message);
                            MessageBox.Show("An error occurred during the deposit. Please try again later.");
                        }
                    }
                }
                else
                {
                    // Connection to the database failed
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., database errors
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }


        //Gebruiker saldo updaten
        private void UpdateSaldoPlus(string rekeningnummer)
        {
            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT saldo FROM Rekeningen WHERE rekeningnummer = @Rekeningnummer";
                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", rekeningnummer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int saldo = reader.GetInt32("saldo");

                            // Update the UI element with the current balance
                            Saldo3.Content = "Je saldo is " + saldo;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred while updating saldo. Please try again later.");
            }
        }

        private void UpdateSaldoMin(string rekeningnummer)
        {
            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT saldo FROM Rekeningen WHERE rekeningnummer = @Rekeningnummer";
                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", rekeningnummer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int saldo = reader.GetInt32("saldo");

                            // Determine whether to add a minus sign based on the saldo
                            string saldoText = saldo < 0 ? "-" + Math.Abs(saldo) : saldo.ToString();

                            // Update the UI element with the current balance
                            Saldo2.Content = "Je saldo is " + saldo;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred while updating saldo. Please try again later.");
            }
        }

        private void Button_Terug(object sender, RoutedEventArgs e)
        {
            string enteredRekeningnummer = RekeningnummerTextBox.Text;

            storten.Visibility = Visibility.Hidden;
            options.Visibility = Visibility.Visible;
            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT saldo FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";

                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int saldo = Convert.ToInt32(reader["saldo"]);
                            SaldoLabel.Content = "Je saldo is " + saldo;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }        
        private void Button_Terug2(object sender, RoutedEventArgs e)
        {
            string enteredRekeningnummer = RekeningnummerTextBox.Text;

            Opnemen.Visibility = Visibility.Hidden;
            options.Visibility = Visibility.Visible;
            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT saldo FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";

                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int saldo = Convert.ToInt32(reader["saldo"]);
                            SaldoLabel.Content = "Je saldo is " + saldo;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }
        public void Button_Transactie(object sender, RoutedEventArgs e)
        {
            string enteredRekeningnummer = RekeningnummerTextBox.Text;

            options.Visibility = Visibility.Hidden;
            Transactie.Visibility = Visibility.Visible;

            myDBconnection dbConnection = new myDBconnection();
            try
            {
                if (dbConnection.Connect())
                {
                    string sql = "SELECT tijd, hoeveel, type FROM transacties WHERE Rekeningen_rekeningnummer = @Rekeningnummer ORDER BY `tijd` DESC LIMIT 3";

                    MySqlCommand command = new MySqlCommand(sql, dbConnection.connection);
                    command.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                    List<Transaction> transactions = new List<Transaction>();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            DateTime time = reader.GetDateTime("tijd");
                            decimal amount = reader.GetDecimal("hoeveel");
                            int type = reader.GetInt32("type");

                            transactions.Add(new Transaction { Time = time, Amount = amount, Type = type });
                        }
                    }

                    // Bind the transactions to the ListBox
                    TransactionListBox.ItemsSource = transactions;
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred. Please try again later.");
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            admindjalla2.Visibility = Visibility.Hidden;
            saldoaanpassen.Visibility = Visibility.Visible;
        }
        private void AddSaldo(string rekeningnummer, decimal amount)
        {
            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    // Update saldo in the database
                    string updateSql = "UPDATE Rekeningen SET saldo = saldo + @Amount WHERE rekeningnummer = @Rekeningnummer";
                    MySqlCommand updateCommand = new MySqlCommand(updateSql, dbConnection.connection);
                    updateCommand.Parameters.AddWithValue("@Amount", amount);
                    updateCommand.Parameters.AddWithValue("@Rekeningnummer", rekeningnummer);
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Saldo added successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to add saldo. Please check the rekeningnummer.");
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred while adding saldo. Please try again later.");
            }
        }
        private void SubtractSaldo(string rekeningnummer, decimal amount)
        {
            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    // Update saldo in the database
                    string updateSql = "UPDATE Rekeningen SET saldo = saldo - @Amount WHERE rekeningnummer = @Rekeningnummer";
                    MySqlCommand updateCommand = new MySqlCommand(updateSql, dbConnection.connection);
                    updateCommand.Parameters.AddWithValue("@Amount", amount);
                    updateCommand.Parameters.AddWithValue("@Rekeningnummer", rekeningnummer);
                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Saldo subtracted successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to subtract saldo. Please check the rekeningnummer or available saldo.");
                    }
                }
                else
                {
                    MessageBox.Show("Database connection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                MessageBox.Show("An error occurred while subtracting saldo. Please try again later.");
            }
        }
        private void Button_SaldoErbij(object sender, RoutedEventArgs e)
        {
            string rekeningnummer = banknummer.Text;
            if (!string.IsNullOrEmpty(rekeningnummer) && decimal.TryParse(saldonummer.Text, out decimal amount))
            {
                AddSaldo(rekeningnummer, amount);
            }
            else
            {
                MessageBox.Show("Invalid rekeningnummer or amount.");
            }
        }

        private void Button_SaldoEraf(object sender, RoutedEventArgs e)
        {
            string rekeningnummer = banknummer.Text;
            if (!string.IsNullOrEmpty(rekeningnummer) && decimal.TryParse(saldonummer.Text, out decimal amount))
            {
                SubtractSaldo(rekeningnummer, amount);
            }
            else
            {
                MessageBox.Show("Invalid rekeningnummer or amount.");
            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            admindjalla2.Visibility = Visibility.Hidden;
            rekeningtoevoegen.Visibility = Visibility.Visible;
        }
        private void Button_Maakrekening(object sender, RoutedEventArgs e)
        {
            string enteredRekeningnummer = rekeningcreate.Text;
            string enteredPincode = pincodecreate.Text;

            if (!string.IsNullOrEmpty(enteredRekeningnummer) && !string.IsNullOrEmpty(enteredPincode))
            {
                try
                {
                    // Hash the pincode using SHA-256
                    string hashedPincode = EncryptString(enteredPincode);

                    myDBconnection dbConnection = new myDBconnection();

                    if (dbConnection.Connect())
                    {
                        // Check if the rekeningnummer already exists in the database
                        string checkSql = "SELECT COUNT(*) FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";
                        MySqlCommand checkCommand = new MySqlCommand(checkSql, dbConnection.connection);
                        checkCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                        long existingAccounts = (long)checkCommand.ExecuteScalar();

                        if (existingAccounts == 0)
                        {
                            // Insert the rekeningnummer and hashed pincode into the database
                            string insertSql = "INSERT INTO Rekeningen (Rekeningnummer, Pincode) VALUES (@Rekeningnummer, @Pincode)";
                            MySqlCommand insertCommand = new MySqlCommand(insertSql, dbConnection.connection);
                            insertCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                            insertCommand.Parameters.AddWithValue("@Pincode", hashedPincode);
                            int rowsAffected = insertCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Rekeningnummer created successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Failed to create rekeningnummer. Please try again.");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Rekeningnummer already exists. Please choose a different one.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Database connection failed.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    MessageBox.Show("An error occurred. Please try again later.");
                }
            }
            else
            {
                MessageBox.Show("Please enter a rekeningnummer and pincode.");
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            admindjalla2.Visibility = Visibility.Hidden;
            rekeningverwijder.Visibility = Visibility.Visible;
        }
    }
}



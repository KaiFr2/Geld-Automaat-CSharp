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
            string enteredRekeningnummer = EncryptString(RekeningnummerTextBox.Text);
            string enteredPincode = EncryptString(PincodeTextBox.Text);

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
                            // Retrieve the encrypted rekeningnummer from the database
                            string storedRekeningnummer = reader["Rekeningnummer"].ToString();
                            string storedPincode = reader["Pincode"].ToString();

                            if (enteredRekeningnummer == storedRekeningnummer && enteredPincode == storedPincode)
                            {
                                int saldo = Convert.ToInt32(reader["saldo"]);

                                MessageBox.Show("Login Successful. Welcome to the ATM!");

                                homescreen.Visibility = Visibility.Hidden;
                                options.Visibility = Visibility.Visible;

                                // Display a message without showing the original rekeningnummer
                                WelcomeLabel.Content = "Welcome to the ATM!";
                                SaldoLabel.Content = "Your saldo: " + saldo;
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

            string enteredRekeningnummer = EncryptString(RekeningnummerTextBox.Text); // Encrypt the input

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
        private int depositCount = 0;

        private void Button_StortGeld100(object sender, RoutedEventArgs e)
        {
            // Check if the user has already made three deposits
            if (depositCount >= 3)
            {
                MessageBox.Show("You have reached the maximum number of allowed deposits (3 times).");
                return;
            }

            string enteredRekeningnummer = EncryptString(RekeningnummerTextBox.Text); // Encrypt the input

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

                            // Record the deposit transaction
                            // Uncomment and modify the transaction insertion code as needed

                            // Record the deposit transaction
                            //  string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, @Tijd, 2, 100)";
                            //  MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            //  insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            //  insertTransactionCommand.ExecuteNonQuery();

                            // Increment the deposit count
                            depositCount++;

                            // Commit the transaction
                            transaction.Commit();

                            // Show a success message
                            MessageBox.Show("Deposit successful. 100 euros added to your account.");

                            // Update the user's saldo label
                            UpdateUserSaldo(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            // Something went wrong, rollback the transaction
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
            if (depositCount >= 3)
            {
                MessageBox.Show("You have reached the maximum number of allowed deposits (3 times).");
                return;
            }

            string enteredRekeningnummer = EncryptString(RekeningnummerTextBox.Text); // Encrypt the input

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

                            // Increment the deposit count
                            depositCount++;

                            // Commit the transaction
                            transaction.Commit();

                            // Show a success message
                            MessageBox.Show("Deposit successful. 200 euros added to your account.");

                            // Update the user's saldo label
                            UpdateUserSaldo(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            // Something went wrong, rollback the transaction
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
            if (depositCount >= 3)
            {
                MessageBox.Show("You have reached the maximum number of allowed deposits (3 times).");
                return;
            }

            string enteredRekeningnummer = EncryptString(RekeningnummerTextBox.Text); // Encrypt the input

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

                            // Increment the deposit count
                            depositCount++;

                            // Commit the transaction
                            transaction.Commit();

                            // Show a success message
                            MessageBox.Show("Deposit successful. 500 euros added to your account.");

                            // Update the user's saldo label
                            UpdateUserSaldo(enteredRekeningnummer);
                        }
                        catch (Exception ex)
                        {
                            // Something went wrong, rollback the transaction
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
        private void UpdateUserSaldo(string rekeningnummer)
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
                            Saldo2.Content = "Saldo: " + reader.GetInt32("saldo") + " euros";
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
    }
}



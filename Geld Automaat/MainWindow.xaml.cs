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
            string enteredPincode = PincodeTextBox.Text;

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
                            string storedRekeningnummer = reader["Rekeningnummer"].ToString();
                            string storedPincode = reader["Pincode"].ToString();

                            if (enteredRekeningnummer == storedRekeningnummer && hashedPincode == storedPincode)
                            {
                                decimal saldo = reader.GetDecimal("saldo");

                                MessageBox.Show("Login Successful. Welcome to the ATM!");

                                homescreen.Visibility = Visibility.Hidden;
                                options.Visibility = Visibility.Visible;

                                WelcomeLabel.Content = "Welcome to the ATM!";
                                SaldoLabel.Content = "Je saldo is " + saldo.ToString("F2"); // Format as a decimal with two decimal places
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
                        // Convert the result to a decimal
                        decimal saldo = Convert.ToDecimal(result);

                        Saldo3.Content = "Je saldo is: " + saldo.ToString("0.00"); // Format as a decimal with two decimal places
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
                        // Convert the result to a decimal
                        decimal saldo = Convert.ToDecimal(result);

                        Saldo2.Content = "Je saldo is: " + saldo.ToString("0.00"); // Format as a decimal with two decimal places
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
        private int StortenCount = 0;

        private void DepositMoney(int amount)
        {
            // Check if the user has already made three deposits
            

            string enteredRekeningnummer = RekeningnummerTextBox.Text;

            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` + @Amount WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";

                            MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                            updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                            updateSaldoCommand.Parameters.AddWithValue("@Amount", amount);

                            updateSaldoCommand.ExecuteNonQuery();

                            string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 2, @Amount)";
                            MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                            insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                            insertTransactionCommand.Parameters.AddWithValue("@Amount", amount);

                            insertTransactionCommand.ExecuteNonQuery();

                            transaction.Commit();
                            MessageBox.Show($"Storten gelukt!. {amount} euros is toegevoegd aan je account.");

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

        private void Button_StortGeld100(object sender, RoutedEventArgs e)
        {
            DepositMoney(100);
        }

        private void Button_StortGeld200(object sender, RoutedEventArgs e)
        {
            DepositMoney(200);
        }

        private void Button_StortGeld500(object sender, RoutedEventArgs e)
        {
            DepositMoney(500);
        }


        private void WithdrawAmount(int amount)
        {

            if (StortenCount >= 3)
            {
                MessageBox.Show("You have reached the maximum number of deposits for the day (3 times).");
                return;
            }
            string enteredRekeningnummer = RekeningnummerTextBox.Text;
            myDBconnection dbConnection = new myDBconnection();

            try
            {
                if (dbConnection.Connect())
                {
                    using (MySqlTransaction transaction = dbConnection.connection.BeginTransaction())
                    {
                        try
                        {
                            string checkSaldoSql = "SELECT saldo FROM `rekeningen` WHERE `rekeningnummer` = @Rekeningnummer";
                            MySqlCommand checkSaldoCommand = new MySqlCommand(checkSaldoSql, dbConnection.connection);
                            checkSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);

                            // ExecuteScalar is used to retrieve the saldo from the database
                            object result = checkSaldoCommand.ExecuteScalar();

                            if (result != null)
                            {
                                decimal saldo = Convert.ToDecimal(result);

                                if (saldo >= amount)
                                {
                                    string updateSaldoSql = $"UPDATE `rekeningen` SET `saldo` = `saldo` - @Amount WHERE `rekeningen`.`rekeningnummer` = @Rekeningnummer";
                                    MySqlCommand updateSaldoCommand = new MySqlCommand(updateSaldoSql, dbConnection.connection);
                                    updateSaldoCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                                    updateSaldoCommand.Parameters.AddWithValue("@Amount", amount);

                                    updateSaldoCommand.ExecuteNonQuery();

                                    string insertTransactionSql = "INSERT INTO transacties (Rekeningen_rekeningnummer, tijd, type, hoeveel) VALUES (@Rekeningnummer, NOW(), 1, @Amount)";
                                    MySqlCommand insertTransactionCommand = new MySqlCommand(insertTransactionSql, dbConnection.connection);
                                    insertTransactionCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                                    insertTransactionCommand.Parameters.AddWithValue("@Amount", amount);

                                    insertTransactionCommand.ExecuteNonQuery();

                                    StortenCount++;

                                    transaction.Commit();
                                    MessageBox.Show($"Opnemen gelukt!. {amount} euros opgenomen van je account.");

                                    UpdateSaldoMin(enteredRekeningnummer);
                                }
                                else
                                {
                                    MessageBox.Show("Je hebt niet genoeg om op te nemen.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Rekeningnummer niet gevonden.");
                            }
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();

                            Console.WriteLine("Error: " + ex.Message);
                            MessageBox.Show("An error occurred during the withdrawal. Please try again later.");
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
            WithdrawAmount(100);
        }

        private void Button_Opnemen200(object sender, RoutedEventArgs e)
        {
            WithdrawAmount(200);
        }

        private void Button_Opnemen500(object sender, RoutedEventArgs e)
        {
            WithdrawAmount(500);
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
                            decimal saldo = reader.GetDecimal("saldo");

                            // Update the UI element with the current balance as a decimal with two decimal places
                            Saldo3.Content = "Je saldo is " + saldo.ToString("F2");
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
                            decimal saldo = reader.GetDecimal("saldo");

                            // Determine whether to add a minus sign based on the saldo
                            string saldoText = saldo < 0 ? "-" + Math.Abs(saldo).ToString("F2") : saldo.ToString("F2");

                            // Update the UI element with the current balance as a decimal with two decimal places
                            Saldo2.Content = "Je saldo is " + saldoText;
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
                            decimal saldo = reader.GetDecimal("saldo");

                            // Update the SaldoLabel with the current balance as a decimal with two decimal places
                            SaldoLabel.Content = "Je saldo is " + saldo.ToString("F2");
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
                            decimal saldo = reader.GetDecimal("saldo");

                            // Update the SaldoLabel with the current balance as a decimal with two decimal places
                            SaldoLabel.Content = "Je saldo is " + saldo.ToString("F2");
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
                    int initialSaldo = 0; 

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
                            // Insert the rekeningnummer, hashed pincode, and initial saldo into the database
                            string insertSql = "INSERT INTO Rekeningen (Rekeningnummer, Pincode, Saldo) VALUES (@Rekeningnummer, @Pincode, @Saldo)";
                            MySqlCommand insertCommand = new MySqlCommand(insertSql, dbConnection.connection);
                            insertCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                            insertCommand.Parameters.AddWithValue("@Pincode", hashedPincode);
                            insertCommand.Parameters.AddWithValue("@Saldo", initialSaldo); // Set the initial saldo
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

        private void Button_Verwijderrekening(object sender, RoutedEventArgs e)
        {
            string enteredRekeningnummer = rekeningremove.Text;
            string enteredPincode = pincoderemove.Text;

            if (!string.IsNullOrEmpty(enteredRekeningnummer) && !string.IsNullOrEmpty(enteredPincode))
            {
                try
                {
                    // Hash the entered pincode using SHA-256
                    string hashedPincode = EncryptString(enteredPincode);

                    myDBconnection dbConnection = new myDBconnection();

                    if (dbConnection.Connect())
                    {
                        // Check if the rekeningnummer and hashed pincode match an existing account
                        string checkSql = "SELECT COUNT(*) FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer AND Pincode = @Pincode";
                        MySqlCommand checkCommand = new MySqlCommand(checkSql, dbConnection.connection);
                        checkCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                        checkCommand.Parameters.AddWithValue("@Pincode", hashedPincode);
                        long matchingAccounts = (long)checkCommand.ExecuteScalar();

                        if (matchingAccounts > 0)
                        {
                            // Execute the SQL statement to remove the specific account from the 'Rekeningen' table
                            string deleteAccountSql = "DELETE FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";
                            MySqlCommand deleteAccountCommand = new MySqlCommand(deleteAccountSql, dbConnection.connection);
                            deleteAccountCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                            int rowsAffected = deleteAccountCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Rekeningnummer removed successfully.");
                            }
                            else
                            {
                                MessageBox.Show("Failed to remove rekeningnummer. Please try again.");
                            }
                        }
                        else 
                        {
                            MessageBox.Show("Rekeningnummer or pincode does not match any existing account.");
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

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            admindjalla2.Visibility = Visibility.Hidden;
            pincodewijzigen.Visibility = Visibility.Visible;
        }

        private void Button_pincodewijzig(object sender, RoutedEventArgs e)
        {
            string enteredRekeningnummer = rekeningkiezen.Text;
            string newPincode = pincodeverander.Password; // Use PasswordBox for pincode input

            if (!string.IsNullOrEmpty(enteredRekeningnummer) && !string.IsNullOrEmpty(newPincode))
            {
                try
                {
                    // Hash the new pincode using SHA-256
                    string hashedPincode = EncryptString(newPincode);

                    myDBconnection dbConnection = new myDBconnection();

                    if (dbConnection.Connect())
                    {
                        // Check if the rekeningnummer exists in the database
                        string checkSql = "SELECT COUNT(*) FROM Rekeningen WHERE Rekeningnummer = @Rekeningnummer";
                        MySqlCommand checkCommand = new MySqlCommand(checkSql, dbConnection.connection);
                        checkCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                        long existingAccounts = (long)checkCommand.ExecuteScalar();

                        if (existingAccounts == 1)
                        {
                            // Update the pincode for the rekeningnummer
                            string updateSql = "UPDATE Rekeningen SET Pincode = @Pincode WHERE Rekeningnummer = @Rekeningnummer";
                            MySqlCommand updateCommand = new MySqlCommand(updateSql, dbConnection.connection);
                            updateCommand.Parameters.AddWithValue("@Rekeningnummer", enteredRekeningnummer);
                            updateCommand.Parameters.AddWithValue("@Pincode", hashedPincode);
                            int rowsAffected = updateCommand.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Pincode successfully updated.");
                            }
                            else
                            {
                                MessageBox.Show("Failed to update pincode. Please try again.");
                            }
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
            else
            {
                MessageBox.Show("Please enter a rekeningnummer and a new pincode.");
            }
        }

        private void Button_Opnemen10(object sender, RoutedEventArgs e)
        {
            WithdrawAmount(10);
        }

        private void Button_Opnemen50(object sender, RoutedEventArgs e)
        {
            WithdrawAmount(50);
        }

        private void Button_Opnemen150(object sender, RoutedEventArgs e)
        {
            WithdrawAmount(150);
        }

        private void Button_Stortgeld10(object sender, RoutedEventArgs e)
        {
            DepositMoney(10);
        }

        private void Button_Stortgeld50(object sender, RoutedEventArgs e)
        {
            DepositMoney(50);
        }

        private void Button_Stortgeld150(object sender, RoutedEventArgs e)
        {
            DepositMoney(150);
        }
    }
}



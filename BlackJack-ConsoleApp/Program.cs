using System;
using Casino;
using System.IO;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;

namespace CardGames_ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════╗");
            Console.WriteLine("║   WELCOME TO BLACKJACK CONSOLE!    ║");
            Console.WriteLine("╚════════════════════════════════════╝");

            Console.WriteLine("Please enter your name:");
            string playerName = Console.ReadLine();
            Console.WriteLine($"Welcome {playerName}!");
            if (playerName.ToLower() == "admin")
            {
                List<ExceptionEntity> Exceptions = RetrieveExceptions();
                foreach (var exception in Exceptions)
                {
                    Console.WriteLine($"ID: {exception.Id} | Type: {exception.ExceptionType} | Message: {exception.ExceptionMessage} | Time: {exception.TimeStamp}");
                }
                Console.ReadLine();
                return;
            }

            Console.WriteLine("Please enter an amount that you would like to start with:");
            int bank = 0;

            bool isValidAnswer = false;
            while (!isValidAnswer)
            {
                isValidAnswer = int.TryParse(Console.ReadLine(), out bank);
                if (!isValidAnswer || bank <= 0) Console.WriteLine("Invalid input. Please enter a positive integer for the amount, no decimals.");
            }
            Console.WriteLine("Hello, {0} would you like to join BlackJack game right now", playerName);
            string answer = Console.ReadLine().ToLower();

            if (answer == "yes" || answer == "yeah" || answer == "y")
            {
                Console.WriteLine("Great! Let's start the game.");
                Player player = new Player(playerName, bank);
                player.Id = Guid.NewGuid();
                using (StreamWriter file = new StreamWriter(@"C:\Users\nasee\source\repos\Basic_C#_Programs folder\BlackJack-ConsoleApp\bin\Debug\log.txt", true))
                {
                    file.WriteLine(new string('=', 60));
                    file.WriteLine("Player Information:");
                    file.WriteLine($"ID: {player.Id}");
                    file.WriteLine($"Name: {player.Name}");
                    file.WriteLine($"Balance: {player.Balance}");
                    file.WriteLine($"Date: {DateTime.Now}");
                    file.WriteLine(new string('=', 60));
                }
                {
                    Game game = new CardGames();
                    game += player;
                    player.isActivelyPlaying = true;

                    while (player.isActivelyPlaying && player.Balance > 0)
                    {
                        try
                        {
                            game.Play();
                        }
                        catch (FraudException ex)
                        {
                            Console.WriteLine(ex.Message);
                            UpdateDBWithException(ex);
                            Console.ReadLine();
                            return;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occured! Please contact your System Administrator.");
                            UpdateDBWithException(ex);
                            Console.ReadLine();
                            return;
                        }
                    }

                    game -= player;

                    // Add this message to inform why the game ended
                    if (player.Balance <= 0)
                    {
                        Console.WriteLine("You've run out of money! Better luck next time.");
                    }
                    Console.WriteLine("Thank you for playing!");
                }

            }
            else
            {
                Console.WriteLine("No problem! Maybe next time.");
                return;
            }
        }
        private static void UpdateDBWithException(Exception ex)
        {
            string connectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=BlackJack;
                                        Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                        TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";

            string queryString = "INSERT INTO Exceptions (ExceptionType, ExceptionMessage, TimeStamp) " +
                "                                 VALUES (@ExceptionType, @ExceptionMessage, @TimeStamp)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                command.Parameters.Add("@ExceptionType", SqlDbType.VarChar).Value = ex.GetType().ToString();
                command.Parameters.Add("@ExceptionMessage", SqlDbType.VarChar).Value = ex.Message;
                command.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = DateTime.Now;

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine("Failed to log exception to database: " + e.Message);
                }
            }
        }
        private static List<ExceptionEntity> RetrieveExceptions()
        {
            string connectionString = @"Data Source=(localdb)\ProjectModels;Initial Catalog=BlackJack;
                                        Integrated Security=True;Connect Timeout=30;Encrypt=False;
                                        TrustServerCertificate=False;ApplicationIntent=ReadWrite;
                                        MultiSubnetFailover=False";
            string queryString = "SELECT Id, ExceptionType, ExceptionMessage, TimeStamp FROM Exceptions";
            List<ExceptionEntity> Exceptions = new List<ExceptionEntity>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Exceptions.Add(new ExceptionEntity()
                        {
                            Id = reader.GetInt32(0),
                            ExceptionType = reader.GetString(1),
                            ExceptionMessage = reader.GetString(2),
                            TimeStamp = reader.GetDateTime(3)
                        });
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to retrieve exceptions from database: " + ex.Message);
                }
            }
            return Exceptions;

        }
    }
}
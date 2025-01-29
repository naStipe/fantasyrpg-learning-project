using fantasyrpg_learning_project.ItemCreator.Models;

namespace fantasyrpg_learning_project;
using Npgsql;


public class DatabaseManager
{
    public DatabaseManager() { }
    
    public void ConnectToPostgres()
    {
        const string connectionString = "Host=localhost;Port=5432;Database=mydatabase;Username=postgres;Password=admin";
    
        using var conn = new NpgsqlConnection(connectionString);
        conn.Open();
        Console.WriteLine("Connection established");
        conn.Close();
    }

    public void SaveItems(Item[] items)
    {
        
    }
}
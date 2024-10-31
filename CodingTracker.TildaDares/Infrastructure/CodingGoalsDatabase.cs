using System.Configuration;
using CodingTracker.TildaDares.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.TildaDares.Infrastructure;

public class CodingGoalsDatabase
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["CodingGoalDB"].ConnectionString;
    
    public CodingGoalsDatabase()
    {
        CreateCodingGoalDb();
    }

    public void InsertCodingGoal(CodingGoal codingGoal)
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = "INSERT INTO codingGoal(startTime, endTime, totalHoursGoal) VALUES (@StartTime, @EndTime, @TotalHoursGoal)";
            var rowsAffected = connection.Execute(sql, codingGoal);
            AnsiConsole.MarkupLine($"[green]{rowsAffected} row(s) inserted.[/]");
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to insert into database. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
    }

    public CodingGoal GetCodingGoal(CodingGoal codingGoal)
    {
        using var connection = new SqliteConnection(_connectionString);
        CodingGoal goal = null;
        try
        {
            connection.Open();
            const string sql = "SELECT * FROM codingGoal WHERE id = @Id";
            goal = connection.QuerySingleOrDefault<CodingGoal>(sql, codingGoal);
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to retrieve coding goal record with ID: {codingGoal.Id}. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
        
        return goal;
    }
    
    public List<CodingGoal> GetAllCodingGoals()
    {
        using var connection = new SqliteConnection(_connectionString);
        var goals = new List<CodingGoal>();
        try
        {
            connection.Open();
            const string sql = "SELECT * FROM codingGoal";
            goals = connection.Query<CodingGoal>(sql).ToList();
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to retrieve all coding goal records. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
        
        return goals;
    }

    public void UpdateCodingGoal(CodingGoal codingGoal)
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = "UPDATE codingGoal SET endTime = @EndTime, totalHoursGoal = @TotalHoursGoal WHERE id = @Id";
            var rowsAffected = connection.Execute(sql, codingGoal);
            AnsiConsole.MarkupLine($"[green]{rowsAffected} row(s) updated.[/]");
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to update coding goal record with ID: {codingGoal.Id}. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
    }

    public void DeleteCodingGoal(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = "DELETE FROM codingGoal WHERE id = @Id";
            var rowsAffected = connection.Execute(sql, new {Id = id});
            AnsiConsole.MarkupLine($"[green]{rowsAffected} row(s) deleted.[/]");
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to delete coding goal record with ID: {id}. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
    }
    
    public long CountCodingGoals()
    {
        using var connection = new SqliteConnection(_connectionString);
        var count = 0;
        try
        {
            connection.Open();
            const string sql = "SELECT COUNT(*) FROM codingGoal";
            count = connection.ExecuteScalar<int>(sql);
        } 
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to count coding goal records. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }

        return count;
    }
    
    private void CreateCodingGoalDb()
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = @" CREATE TABLE IF NOT EXISTS codingGoal (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                startTime TEXT NOT NULL,
                endTime Text NOT NULL,
                totalHoursGoal TEXT NOT NULL DEFAULT '0.00')";
            connection.Execute(sql);
            
            // Check if table is already populated
            const string selectSql = "SELECT COUNT(*) FROM codingGoal";
            var count = connection.ExecuteScalar<int>(selectSql);
            
            if (count == 0)
            {
                SeedCodingGoalDb();
            }
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to create coding goal database. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
    }
    
    private void SeedCodingGoalDb()
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            var rand = new Random();

            for (var i = 0; i < 5; i++)
            {
                var startTime = Utilities.GetRandomDateTime(rand);
                var endTime = Utilities.GetRandomDateTime(rand, startTime);
                var totalHoursGoal = rand.NextDouble() * 100;
                const string sql = "INSERT INTO codingGoal(startTime, endTime, totalHoursGoal) VALUES (@StartTime, @EndTime, @TotalHoursGoal)";
                
                var codingGoal = new CodingGoal { StartTime = startTime, EndTime = endTime, TotalHoursGoal = totalHoursGoal };
                connection.Execute(sql, codingGoal);
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to seed coding goal database. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
}

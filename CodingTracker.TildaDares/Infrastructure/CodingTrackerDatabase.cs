using System.Configuration;
using CodingTracker.TildaDares.Models;
using Dapper;
using Microsoft.Data.Sqlite;
using Spectre.Console;

namespace CodingTracker.TildaDares.Infrastructure;

public class CodingTrackerDatabase
{
    private readonly string _connectionString = ConfigurationManager.ConnectionStrings["CodingTrackerDB"].ConnectionString;
    
    public CodingTrackerDatabase()
    {
        CreateCodingTrackerDb();
    }

    public void InsertCodingSession(CodingSession codingSession)
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = "INSERT INTO codingTracker(startTime, endTime, duration) VALUES (@StartTime, @EndTime, @Duration)";
            var rowsAffected = connection.Execute(sql, codingSession);
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

    public CodingSession GetCodingSession(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        CodingSession session = null;
        try
        {
            connection.Open();
            const string sql = "SELECT * FROM codingTracker WHERE id = @Id";
            session = connection.QuerySingleOrDefault<CodingSession>(sql, new {Id = id});
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to retrieve coding session record with ID: {id}. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
        
        return session;
    }

    public CodingStats GetSumOfCodingSessionDuration(CodingSessionFilter filter)
    {
        using var connection = new SqliteConnection(_connectionString);
        CodingStats stats = null;
        try
        {
            connection.Open();
            var sql = "SELECT printf('%.2f', SUM(duration)) AS TotalHours, COUNT(*) As RecordCount FROM codingTracker";
            if (filter is { StartTime: not null, EndTime: not null })
            {
                sql += " WHERE DATETIME(startTime) >= DATETIME(@StartTime) AND DATETIME(endTime) <= DATETIME(@EndTime)";
            }

            if (filter.StartTime != null)
                stats = connection.QuerySingle<CodingStats>(sql,
                    new { StartTime = filter.StartTime.Value, EndTime = filter.EndTime.Value });
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to retrieve all coding session records. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
        
        return stats;
    }
    
    public List<CodingSession> GetAllCodingSessions(CodingSessionFilter? filter = null)
    {
        using var connection = new SqliteConnection(_connectionString);
        var sessions = new List<CodingSession>();
        try
        {
            connection.Open();
            var sql = "SELECT * FROM codingTracker";
            if (filter is { StartTime: not null, EndTime: not null })
            {
                sql += " WHERE DATETIME(startTime) >= DATETIME(@StartTime) AND DATETIME(endTime) <= DATETIME(@EndTime)";
            }
            
            sessions = connection.Query<CodingSession>(sql, filter).ToList();
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to retrieve all coding session records. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
        
        return sessions;
    }

    public void UpdateCodingSession(CodingSession codingSession)
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = "UPDATE codingTracker SET startTime = @StartTime, endTime = @EndTime, duration = @Duration WHERE id = @Id";
            var rowsAffected = connection.Execute(sql, codingSession);
            AnsiConsole.MarkupLine($"[green]{rowsAffected} row(s) updated.[/]");
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to update coding session record with ID: {codingSession.Id}. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
    }

    public void DeleteCodingSession(int id)
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = "DELETE FROM codingTracker WHERE id = @Id";
            var rowsAffected = connection.Execute(sql, new {Id = id});
            AnsiConsole.MarkupLine($"[green]{rowsAffected} row(s) deleted.[/]");
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to delete coding session record with ID: {id}. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
    }
    
    public long CountCodingSessions()
    {
        using var connection = new SqliteConnection(_connectionString);
        var count = 0;
        try
        {
            connection.Open();
            const string sql = "SELECT COUNT(*) FROM codingTracker";
            count = connection.ExecuteScalar<int>(sql);
        } 
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to count coding session records. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }

        return count;
    }
    
    private void CreateCodingTrackerDb()
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            const string sql = @" CREATE TABLE IF NOT EXISTS codingTracker (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                startTime TEXT NOT NULL,
                endTime Text NOT NULL,
                duration TEXT NOT NULL DEFAULT '0.00')";
            connection.Execute(sql);
            
            // Check if table is already populated
            const string selectSql = "SELECT COUNT(*) FROM codingTracker";
            var count = connection.ExecuteScalar<int>(selectSql);
            
            if (count == 0)
            {
                SeedCodingTrackerDb();
            }
        }
        catch (SqliteException e)
        {
            AnsiConsole.MarkupLine($"[red]Unable to create coding tracker database. {e.Message}[/]");
        }
        finally
        {
            connection.Close();
        }
    }
    
    private void SeedCodingTrackerDb()
    {
        using var connection = new SqliteConnection(_connectionString);
        try
        {
            connection.Open();
            var rand = new Random();

            for (var i = 0; i < 15; i++)
            {
                var startTime = Utilities.GetRandomDateTime(rand);
                var endTime = Utilities.GetRandomDateTime(rand, startTime);
                const string sql = "INSERT INTO codingTracker(startTime, endTime, duration) VALUES (@StartTime, @EndTime, @Duration)";
                
               var codingSession = new CodingSession(startTime, endTime);
               connection.Execute(sql, codingSession);
            }
        }
        catch (SqliteException e)
        {
            Console.WriteLine($"Unable to seed coding tracker database. {e.Message}");
        }
        finally
        {
            connection.Close();
        }
    }
}

using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using Spectre.Console;

namespace CodingTracker.TildaDares.Services;

public class StopwatchService
{
    private static readonly string DateFormat = ConfigurationManager.AppSettings["dateFormat"];
    private readonly Stopwatch _stopwatch = new();
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public TimeSpan Duration => EndTime.Subtract(StartTime).Duration();

    public void Start()
    {
        if (_stopwatch.IsRunning)
        {
            AnsiConsole.MarkupLine("[green]Stopwatch is already running![/]");
            return;
        }
        AnsiConsole.MarkupLine("[green]The stopwatch is starting...![/]");
        _stopwatch.Start();
        var startTimeString = DateTime.Now.ToString(DateFormat);
        StartTime = DateTime.ParseExact(startTimeString, DateFormat, new CultureInfo("en-US"));
    }
    
    public void Stop()
    {
        if (!_stopwatch.IsRunning)
        {
            AnsiConsole.MarkupLine("[green]Stopwatch has already stopped running![/]");
            return;
        }
        AnsiConsole.MarkupLine("[green]The stopwatch is stopping...![/]");
        _stopwatch.Stop();
        var endTimeString = DateTime.Now.ToString(DateFormat);
        EndTime = DateTime.ParseExact(endTimeString, DateFormat, new CultureInfo("en-US"));
    }
}

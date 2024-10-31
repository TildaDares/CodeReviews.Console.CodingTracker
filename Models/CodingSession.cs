namespace CodingTracker.TildaDares.Models;

public class CodingSession
{
    public CodingSession() {}
    public CodingSession(DateTime startTime, DateTime endTime)
    {
        StartTime = startTime;
        EndTime = endTime;
        CalculateDuration();
    }
    
    public int Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public double Duration { get; private set; }

    public void CalculateDuration()
    {
        Duration = Math.Round(EndTime.Subtract(StartTime).TotalHours, 2);
    }
}

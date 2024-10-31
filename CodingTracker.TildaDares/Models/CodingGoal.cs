namespace CodingTracker.TildaDares.Models;

public class CodingGoal
{
    public int Id { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public double TotalHoursGoal { get; init; }
}

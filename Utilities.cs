namespace CodingTracker.TildaDares;

public static class Utilities
{
    public static DateTime GetRandomDateTime(Random random)
    {
        var twoYearsAgo = DateTime.Now.AddYears(-2).Year;
        var date = new DateTime(twoYearsAgo, 1, 1);
        var now = DateTime.Now;

        var range = now.Subtract(date);
        var days = random.Next(0, range.Days);
        return date.AddDays(days);
    }

    public static DateTime GetRandomDateTime(Random random, DateTime startTime)
    {
        var range = DateTime.Now.Subtract(startTime);
        var days = random.Next(0, range.Days);
        return startTime.AddDays(days);
    }
}

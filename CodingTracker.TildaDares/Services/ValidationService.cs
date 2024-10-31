using System.Configuration;
using System.Globalization;
using CodingTracker.TildaDares.Models;

namespace CodingTracker.TildaDares.Services;

public static class ValidationService
{
    private static readonly string DateFormat = ConfigurationManager.AppSettings["dateFormat"];
    public static Validator IsDateValid(string dateString, DateTime? minRange = null, DateTime? maxRange = null)
    {
        minRange ??= DateTime.MinValue;
        maxRange ??= DateTime.MaxValue;
        var enUS = new CultureInfo("en-US");
        var isValid = DateTime.TryParseExact(dateString, DateFormat, enUS,
            DateTimeStyles.None, out var dateValue);
        var isWithinRange = dateValue >= minRange && dateValue <= maxRange;

        var message = "";
        if (!isValid) message = "Invalid format";
        else if (!isWithinRange) message = "Date is out of range!";
        
        return new Validator(isValid && isWithinRange, message, dateTime: dateValue);
    }
    
    public static bool DeadlinePassed(DateTime endTime)
    {
        return DateTime.Now >= endTime;
    }
}

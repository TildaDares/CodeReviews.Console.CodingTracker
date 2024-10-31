namespace CodingTracker.TildaDares.Models;

public class Validator(bool isValid = false, string? message = "", DateTime? dateTime = null)
{
    public bool IsValid { get; } = isValid;
    public string? Message { get; } = message;
    public DateTime? DateTime { get; } = dateTime;
}

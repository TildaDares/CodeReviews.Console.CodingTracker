namespace CodingTracker.TildaDares.Models;

public class Validator(bool isValid = false, string? message = "", DateTime? dateTime = null)
{
    public bool IsValid { get; init; } = isValid;
    public string Message { get; init; } = message;
    public DateTime? DateTime { get; init; } = dateTime;
}

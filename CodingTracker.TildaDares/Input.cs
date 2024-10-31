using CodingTracker.TildaDares.Models;
using CodingTracker.TildaDares.Services;
using Spectre.Console;

namespace CodingTracker.TildaDares;

public static class Input
{
    public static DateTime GetDateInput(string message, DateTime? minRange = null, DateTime? maxRange = null)
    {
        Console.Clear();
        var validator = new Validator();
        AnsiConsole.Prompt(
            new TextPrompt<string>(message)
                .Validate((dateString) =>
                {
                    validator = ValidationService.IsDateValid(dateString, minRange, maxRange);
                    return validator.IsValid ? ValidationResult.Success() : ValidationResult.Error(validator.Message); 
                }));

        return validator.DateTime.Value;
    }
    
    public static void ContinueMenu()
    {
        AnsiConsole.MarkupLine("\n[green]Press any key to continue...[/]");
        Console.ReadLine();
    }

    public static bool ConfirmPrompt(string message)
    {
        var confirmation = AnsiConsole.Prompt(
            new ConfirmationPrompt($"[yellow]{message}[/]"));

        return confirmation;
    }
}

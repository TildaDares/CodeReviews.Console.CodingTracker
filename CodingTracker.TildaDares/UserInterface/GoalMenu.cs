using CodingTracker.TildaDares.Enums.Models;
using CodingTracker.TildaDares.Enums.Services;
using CodingTracker.TildaDares.Infrastructure;
using CodingTracker.TildaDares.Models;
using CodingTracker.TildaDares.Services;
using Spectre.Console;

namespace CodingTracker.TildaDares.UserInterface;

public class GoalMenu(CodingTrackerDatabase _trackerDatabase)
{
   private readonly CodingGoalsDatabase _goalsDatabase = new();

   public void Show()
   {
      var exit = false;
      while (!exit)
      {
         Console.Clear();
         var choice = AnsiConsole.Prompt(
            new SelectionPrompt<GoalMenuOptions>()
               .Title("Select an option:")
               .UseConverter(g => $"{g.GetDescription()}")
               .AddChoices(Enum.GetValues<GoalMenuOptions>()));

         switch (choice)
         {
            case GoalMenuOptions.InsertCodingGoal:
               InsertCodingGoal();
               break;
            case GoalMenuOptions.GetCodingGoal:
               GetCodingGoal();
               break;
            case GoalMenuOptions.GetCodingGoals:
               GetCodingGoals();
               break;
            case GoalMenuOptions.UpdateCodingGoal:
               UpdateCodingGoal();
               break;
            case GoalMenuOptions.DeleteCodingGoal:
               DeleteCodingGoal();
               break;
            case GoalMenuOptions.Exit:
            default:
               exit = true;
               break;
         }
      }
   }

   private void InsertCodingGoal()
   {
      Console.Clear();
      AnsiConsole.MarkupLine("[bold yellow]Your coding goal startTime starts counting when you successfully set a goal[/]");
      AnsiConsole.MarkupLine("[bold yellow]Your coding goal endTime must be set in the future[/]");
      Input.ContinueMenu();
      
      var startTime = DateTime.Now;
      var endTime = Input.GetDateInput(
         "[green]Enter the end date & time of your coding goal in the format[/] [blue]dd/mm/yyyy HH:mm (24-hour format only)[/]:\n", minRange: startTime);
      
      var goalHours = AnsiConsole.Ask<double>("[green]Enter the number of hours of your coding goal:[/]");
      
      var confirmation = Input.ConfirmPrompt("[yellow]Save coding goal to database?[/]");
      if (!confirmation) return;

      var codingGoal = new CodingGoal() { StartTime = startTime, EndTime = endTime, TotalHoursGoal = goalHours };
      _goalsDatabase.InsertCodingGoal(codingGoal);
      Input.ContinueMenu();
   }

   private void GetCodingGoal()
   {
      Console.Clear();
      if (!HasCodingGoals())
      {
         Input.ContinueMenu();
         return;
      }
    
      GetCodingGoals();
      var id = AnsiConsole.Ask<int>("Enter the coding goal ID you wish to retrieve:");
      var goal = new CodingGoal() { Id = id };
      goal = _goalsDatabase.GetCodingGoal(goal);

      if (goal == null)
      {
         AnsiConsole.MarkupLine("[red]No coding goal found with that ID![/]");
         Input.ContinueMenu();
         return;
      }
    
      Console.Clear();
      var panel = new Panel("Coding Goal Record:")
      {
         Border = BoxBorder.Ascii
      };
      
      var filter = new CodingSessionFilter { StartTime = goal.StartTime, EndTime = goal.EndTime };
      var stats = _trackerDatabase.GetSumOfCodingSessionDuration(filter);
      var table = new Table();
      BuildTableHeader(table);
      BuildTableRows(table, goal);
      
      AnsiConsole.Render(table);
      DisplayCodingGoalDetails(goal, stats.TotalHours);
      Input.ContinueMenu();
   }
   
   private void GetCodingGoals()
   {
      Console.Clear();
      var goals = _goalsDatabase.GetAllCodingGoals();
      if (goals.Count == 0)
      {
         AnsiConsole.MarkupLine("[green]No coding goals found![/]");
         Input.ContinueMenu();
         return;
      }
      
      var panel = new Panel("All Coding Goal records:")
      {
         Border = BoxBorder.Ascii
      };
      var table = new Table();
      BuildTableHeader(table);
    
      foreach (var goal in goals)
      {
         BuildTableRows(table, goal);
      }
        
      AnsiConsole.Write(panel);
      AnsiConsole.Write(table);
      AnsiConsole.MarkupLine("[bold yellow]* All rows colored [red]red[/] are [red]COMPLETED/PAST[/] goals:[/]");
      Input.ContinueMenu();
   }
   
   private void UpdateCodingGoal()
   {
      Console.Clear();
      if (!HasCodingGoals())
      {
         Input.ContinueMenu();
         return;
      }
      
      GetCodingGoals();
      var id = AnsiConsole.Ask<int>("Enter the coding goal ID you wish to update:");
      var goal = new CodingGoal() { Id = id };
      goal = _goalsDatabase.GetCodingGoal(goal);

      if (goal == null)
      {
         AnsiConsole.MarkupLine("[red]No coding goal found with that ID![/]");
         Input.ContinueMenu();
         return;
      }
        
      AnsiConsole.MarkupLine("[bold yellow]Start time & date cannot be updated![/]");
      var endTime = Input.GetDateInput(
         "[green]Enter the updated end date & time of your coding goal in the format[/] [blue]dd/mm/yyyy HH:mm (24-hour format only)[/]:\n", minRange: goal.StartTime);
      
      var goalHours = AnsiConsole.Ask<double>("[green]Enter the updated number of hours of your coding goal:[/]");

      var confirmation = Input.ConfirmPrompt("[yellow]Save updated coding goal to database?[/]");
      if (!confirmation) return;
      
      var updatedCodingGoal = new CodingGoal() { Id = goal.Id, EndTime = endTime, TotalHoursGoal = goalHours };
      _goalsDatabase.UpdateCodingGoal(updatedCodingGoal);
      
      Input.ContinueMenu();
   }
   
   private void DeleteCodingGoal()
   {
      Console.Clear();
      if (!HasCodingGoals())
      {
         Input.ContinueMenu();
         return;
      }
    
      GetCodingGoals();
      var id = AnsiConsole.Ask<int>("Enter the coding goal ID you wish to delete:");
      var confirmation = Input.ConfirmPrompt("[yellow]This action is irreversible. Confirm delete?[/]");
      if (!confirmation) return;
      
      _goalsDatabase.DeleteCodingGoal(id);
      Input.ContinueMenu();
   }
   
   private bool HasCodingGoals()
   {
      var count = _goalsDatabase.CountCodingGoals();
      if (count >= 1) return true;
      AnsiConsole.MarkupLine("[green]No coding goals found![/]");
      return false;
   }
   
   private static void BuildTableHeader(Table table)
   {
      table.AddColumn(new TableColumn("[yellow]Id[/]").Centered());
      table.AddColumn(new TableColumn("[yellow]StartTime[/]").Centered());
      table.AddColumn(new TableColumn("[yellow]EndTime[/]").Centered());
      table.AddColumn(new TableColumn("[yellow]TotalHoursGoal[/]").Centered());
   }
    
   private static void BuildTableRows(Table table, CodingGoal codingGoal)
   {
      var color = ValidationService.DeadlinePassed(codingGoal.EndTime) ? "[red]" : "[blue]";
      table.AddRow($"{color}{codingGoal.Id}[/]", $"{color}{codingGoal.StartTime}[/]", $"{color}{codingGoal.EndTime}[/]", $"{color}{codingGoal.TotalHoursGoal}[/]");
   }

   private static void DisplayCodingGoalDetails(CodingGoal goal, double totalHours)
   {
      var completedHoursPercentage = Math.Round((totalHours / goal.TotalHoursGoal) * 100, 2);
      var remainingHoursPercentage = Math.Round(100 - completedHoursPercentage, 2);
      
      if (ValidationService.DeadlinePassed(goal.EndTime))
      {
         AnsiConsole.MarkupLine("[red]Goal Deadline Passed![/]");
         var message = $"[blue]You completed {totalHours:F} / {goal.TotalHoursGoal:F} hours.";
         AnsiConsole.MarkupLine(totalHours >= goal.TotalHoursGoal
            ? $"{message} You reached your coding goal!![/]"
            : $"{message} Coding goal was not completed![/]");
      }
      else
      {
         if (totalHours >= goal.TotalHoursGoal)
         {
            AnsiConsole.MarkupLine($"[blue]You completed {totalHours:F} / {goal.TotalHoursGoal:F} hours. Congratulations on reaching your coding goal!![/]");
         }
         else
         {
            var remainingHours = goal.TotalHoursGoal - totalHours;
            var daysRemaining = goal.EndTime.Subtract(DateTime.Now).TotalDays;
            var hoursToCompletePerDay = remainingHours / daysRemaining;
            
            AnsiConsole.MarkupLine($"[blue]You have coded for [bold][yellow]{totalHours:F}[/][/]/[bold][yellow]{goal.TotalHoursGoal:F}[/][/] hours required in this coding goal.[/]");
            AnsiConsole.MarkupLine($"[blue]To reach your coding goal, you would have to code for [bold][yellow]{hoursToCompletePerDay:F}[/][/] hours each day until [bold][yellow]{goal.EndTime}[/][/][/]");
         }
      }

      AnsiConsole.Write(new BreakdownChart()
         .Width(60)
         .ShowPercentage()
         .AddItem("Completed Hours", completedHoursPercentage, Color.Green)
         .AddItem("Remaining Hours", remainingHoursPercentage, Color.Red));
   }
}

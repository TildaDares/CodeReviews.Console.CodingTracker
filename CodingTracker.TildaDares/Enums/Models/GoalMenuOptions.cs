using System.ComponentModel;

namespace CodingTracker.TildaDares.Enums.Models;

public enum GoalMenuOptions
{
    [Description("Set coding goal")]
    InsertCodingGoal = 0,
    [Description("Get coding goal")]
    GetCodingGoal,
    [Description("Get coding goals")]
    GetCodingGoals,
    [Description("Update coding goal")]
    UpdateCodingGoal,
    [Description("Delete coding goal")]
    DeleteCodingGoal,
    [Description("Exit")]
    Exit
}

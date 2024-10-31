using System.ComponentModel;

namespace CodingTracker.TildaDares.Enums.Models;

public enum MainMenuOptions
{
    [Description("Insert coding session")]
    InsertCodingSession = 0,
    [Description("Get coding session")]
    GetCodingSession,
    [Description("Get coding sessions")]
    GetCodingSessions,
    [Description("Update coding session")]
    UpdateCodingSession,
    [Description("Delete coding session")]
    DeleteCodingSession,
    [Description("Start coding session")]
    StartCodingSession,
    [Description("Filter coding sessions by period")]
    FilterCodingSessionsByPeriod,
    [Description("View coding session report by period")]
    ViewCodingSessionReportByPeriod,
    [Description("Goals")]
    Goals,
    [Description("Exit")]
    Exit
}

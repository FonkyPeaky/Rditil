using System.Collections.Generic;
using Rditil.Models;

namespace Rditil.Services
{
    public interface IAppState
    {
        Utilisateur? CurrentUser { get; set; }
        string? ManagerEmail { get; set; }
        IReadOnlyList<ExamReportRow>? LastExamRows { get; set; }
        ExamResultSummary? LastExamResult { get; set; }
    }

    public sealed class AppState : IAppState
    {
        public Utilisateur? CurrentUser { get; set; }
        public string? ManagerEmail { get; set; }
        public IReadOnlyList<ExamReportRow>? LastExamRows { get; set; }
        public ExamResultSummary? LastExamResult { get; set; }
    }
}

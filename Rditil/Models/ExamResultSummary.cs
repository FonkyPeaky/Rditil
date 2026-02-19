using System;

namespace Rditil.Models
{
    public class ExamResultSummary
    {
        public string UserFullName { get; set; } = "";
        public string UserEmail { get; set; } = "";
        public string ManagerEmail { get; set; } = "";

        public int Score { get; set; }
        public int Total { get; set; }

        public TimeSpan Duration => FinishedAt - StartedAt;

        public DateTime StartedAt { get; set; }
        public DateTime FinishedAt { get; set; }
    }
}

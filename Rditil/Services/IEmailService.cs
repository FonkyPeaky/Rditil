using Rditil.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Rditil.Services
{
    public interface IEmailService
    {
        Task SendExamReportAsync(
            string to,
            string? cc,
            string subject,
            string userFullName,
            int score,
            int total,
            TimeSpan duration,
            IReadOnlyList<ExamReportRow> rows);
    }
}

namespace Rditil.Models
{
    public class ExamReportRow
    {
        public string Enonce { get; set; } = "";
        public string ChosenText { get; set; } = "";
        public bool IsCorrect { get; set; }
        public string? CorrectText { get; set; }
    }
}

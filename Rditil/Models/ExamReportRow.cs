namespace Rditil.Models
{
    public class ExamReportRow
    {
        public int Index { get; set; }
        public string Enonce { get; set; } = "";
        public string ChosenText { get; set; } = "";
        public string CorrectText { get; set; } = "";
        public bool IsCorrect { get; set; }
    }
}

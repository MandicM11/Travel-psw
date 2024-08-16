namespace Travel_psw.Models
{
    public class Report
    {
        public List<UserReport> MonthlyReports { get; set; }
        public ReportDto Summary { get; set; }
    }
}

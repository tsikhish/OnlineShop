namespace OnlineShops.Models.DbModels
{
    public class ReportsViewModel
    {
        public string SelectedReport { get; set; }
        public IEnumerable<dynamic> ReportData { get; set; }
    }
}

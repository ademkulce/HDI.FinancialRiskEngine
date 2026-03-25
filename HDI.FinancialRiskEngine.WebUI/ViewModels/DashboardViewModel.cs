namespace HDI.FinancialRiskEngine.WebUI.ViewModels
{
    public class DashboardViewModel
    {
        // Dashboard da gösterilecek toplam sayılar
        public int TotalAgreements { get; set; }
        public int TotalBusinessPartners { get; set; }
        public int TotalBusinessTopics { get; set; }
        public int TotalAnalyzedTopics { get; set; }

        // Risk seviyelerine göre dağılım bilgileri
        public int LowRiskCount { get; set; }
        public int MediumRiskCount { get; set; }
        public int HighRiskCount { get; set; }
        public int CriticalRiskCount { get; set; }

        // Son eklenen konular için bir liste
        public List<RecentBusinessTopicViewModel> RecentBusinessTopics { get; set; } = new();
    }

    public class RecentBusinessTopicViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? ReferenceNumber { get; set; }
        public DateTime TopicDate { get; set; }
        public string Status { get; set; } = null!;
        public decimal? CalculatedRiskAmount { get; set; }
    }
}

namespace FreelanceProject.Models.ViewModels
{
    public class PaymentSummaryViewModel
    {
        public Guid JobId { get; set; }
        public Guid UserId { get; set; }

        public decimal Budget { get; set; }
        public decimal SiteCommission { get; set; }
        public decimal TotalAmount { get; set; }

        public string JobTitle { get; set; }
    }
}

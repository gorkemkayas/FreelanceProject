namespace FreelanceProject.Models.ViewModels
{
    public class CreateJobViewModel
    {
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public decimal JobBudget { get; set; }
        public int JobDuration { get; set; }
        public string DurationUnit { get; set; }
        public DateTime StartDate { get; set; }
        public List<string> Skills { get; set; } = new List<string>();
    }
}

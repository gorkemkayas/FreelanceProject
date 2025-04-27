using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class JobEntity : BaseEntity
    {
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public decimal JobBudget { get; set; }
        public int JobDuration { get; set; }
        public string DurationUnit { get; set; }
        public DateTime StartDate { get; set; }
        public string Skills { get; set; } // Virgülle ayrılmış string (HTML/CSS, JavaScript gibi)
        public string EmployerId { get; set; }  // İşverenin UserId'si

    }
}

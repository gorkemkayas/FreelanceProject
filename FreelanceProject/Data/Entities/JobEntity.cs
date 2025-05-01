using FreelanceProject.Data.Entities.BaseEntities;
using Microsoft.AspNetCore.Builder;
using System.Diagnostics.Contracts;

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
        // public string Skills { get; set; } // Virgülle ayrılmış string (HTML/CSS, JavaScript gibi)
        public bool IsActive { get; set; } = true;

        // İşveren
        public Guid EmployerId { get; set; }
        public AppUser Employer { get; set; } = null!;

        // Başvurular (freelancer'ların yaptığı başvurular)
        //public virtual ICollection<Application> Applications { get; set; } = new List<JobApplication>();

        // Kabul edilen iş varsa ilişki kurulabilir (opsiyonel)
        //public virtual Contract? Contract { get; set; }

        public string Category { get; set; } // Kategori adını burada tutuyoruz
    }
}

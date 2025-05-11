using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class JobApplicationEntity : BaseEntity
    {
        public Guid JobId { get; set; } // Başvurulan işin ID'si
        public virtual JobEntity Job { get; set; } // Başvurulan iş ile ilişki (Navigation Property)

        public Guid ApplicantId { get; set; } // Başvuran kullanıcının ID'si
        public virtual AppUser Applicant { get; set; } // Başvuran kullanıcı ile ilişki (Navigation Property)

        public DateTime AppliedDate { get; set; } = DateTime.UtcNow; // Başvuru tarihi

        // Başvuru durumu enum
        public JobApplicationStatus Status { get; set; } = JobApplicationStatus.Pending;

        public bool IsApprovedByEmployer { get; set; } = false; // İşveren onayı
        public bool IsApprovedByApplicant { get; set; } = false; // Başvuru sahibinin onayı

        public bool IsAccepted => IsApprovedByEmployer && IsApprovedByApplicant; // Her iki tarafın onayı ile kabul durumu

        public string? CoverLetter { get; set; } // Başvuru mesajı (Kapak Mektubu)
        public DateTime? DeletedDate { get; set; } // Silindiği tarih
    }
    public enum JobApplicationStatus
    {
        Pending = 0, // Başvuru beklemede
        Accepted = 1, // Başvuru kabul edildi
        Rejected = 2, // Başvuru reddedildi
        Ongoing = 3, // Başvuru devam ediyor
        Completed = 4 // Başvuru tamamlandı
    }
}
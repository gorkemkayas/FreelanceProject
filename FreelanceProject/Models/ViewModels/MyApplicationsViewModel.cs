using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class MyApplicationsViewModel
    {
        public Guid Id { get; set; }

        // İş Başlığı
        [Required(ErrorMessage = "İş başlığı zorunludur.")]
        public string JobTitle { get; set; } = null!;

        // Kategori
        [Required(ErrorMessage = "Kategori zorunludur.")]
        public string JobCategory { get; set; } = null!;

        // Bütçe
        [Required(ErrorMessage = "Bütçe zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Geçerli bir bütçe giriniz.")]
        public decimal JobBudget { get; set; }

        // Açıklama
        [Required(ErrorMessage = "İş açıklaması zorunludur.")]
        public string JobDescription { get; set; } = null!;

        // Gereksinimler
        public string JobRequirements { get; set; }

        // İş Resmi
        [Required(ErrorMessage = "İş resmi zorunludur.")]
        public string JobImage { get; set; } = null!;

        // Başlangıç Tarihi
        [Required(ErrorMessage = "Başlama tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public string JobStartDate { get; set; } = null!;

        // Bitiş Tarihi
        public string JobDeadline { get; set; }

        // Aktiflik Durumu
        public bool JobIsActive { get; set; }

        // Tamamlanma Durumu
        public bool JobIsCompleted { get; set; }

        // Başvuru ID
        public Guid ApplicationId { get; set; }

        // Başvuran ID
        [Required(ErrorMessage = "Başvuran ID zorunludur.")]
        public Guid ApplicantId { get; set; }

        // Başvuru Tarihi
        [Required(ErrorMessage = "Başvuru tarihi zorunludur.")]
        public DateTime AppliedDate { get; set; }

        // Başvuru Durumu
        [Required(ErrorMessage = "Başvuru durumu zorunludur.")]
        public string ApplicationStatus { get; set; } = null!;

        // Kapak Mektubu
        public string CoverLetter { get; set; } = null!;
        public Guid JobId { get; internal set; }
    }
}

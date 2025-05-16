using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class EditJobViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "İş başlığı zorunludur.")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "İş açıklaması zorunludur.")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } = null!;

        public string? Requirements { get; set; }

        [Required(ErrorMessage = "Bütçe zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Geçerli bir bütçe giriniz.")]
        public decimal Budget { get; set; }

        //[Required(ErrorMessage = "Süre zorunludur.")]
        //public int Duration { get; set; }

        //[Required(ErrorMessage = "Süre birimi zorunludur.")]
        //public string DurationUnit { get; set; } = "Gün";

        [Required(ErrorMessage = "Bitiş tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime? Deadline { get; set; }

        [Required(ErrorMessage = "Başlama tarihi zorunludur.")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        // Kategori adını string olarak alacağız.
        [Required(ErrorMessage = "Kategori seçimi zorunludur.")]
        public string Category { get; set; } = null!;

        //public string ImageUrl { get; set; } // Web URL

        public IFormFile? ImageFile { get; set; } // Lokal dosya yükleme
        public object? ImageUrl { get; internal set; }
    }
}

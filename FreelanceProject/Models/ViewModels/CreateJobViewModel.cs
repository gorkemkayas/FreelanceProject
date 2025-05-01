using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class CreateJobViewModel
    {
        public Guid? JobId { get; set; }


        [Required(ErrorMessage = "İş başlığı zorunludur.")]
        public string JobTitle { get; set; } = null!;

        [Required(ErrorMessage = "İş açıklaması zorunludur.")]
        [DataType(DataType.MultilineText)]
        public string JobDescription { get; set; } = null!;

        [Required(ErrorMessage = "Bütçe zorunludur.")]
        [Range(0, double.MaxValue, ErrorMessage = "Geçerli bir bütçe giriniz.")]
        public decimal JobBudget { get; set; }

        [Required(ErrorMessage = "Süre zorunludur.")]
        public int JobDuration { get; set; }

        [Required(ErrorMessage = "Süre birimi zorunludur.")]
        public string DurationUnit { get; set; } = "Gün";

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

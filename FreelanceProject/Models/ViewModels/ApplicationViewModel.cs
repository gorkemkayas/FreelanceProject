using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class ApplicationViewModel
    {
        public Guid ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public DateTime AppliedDate { get; set; }
        public string Message { get; set; }
    }
}

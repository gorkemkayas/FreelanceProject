using FreelanceProject.Data.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using FreelanceProject.Data.Entities;

namespace FreelanceProject.Models.ViewModels
{
    public class ApplicantViewModel
    {
        public Guid ApplicantId { get; set; }
        public string UserName { get; set; }

        public string FullName { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public JobApplicationStatus Status { get; set; }
        public bool IsApprovedByEmployer { get; set; }
        public bool IsApprovedByApplicant { get; set; }
        public string Email { get; set; }
        public string CVPath { get; set; }

    }

}

using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class ForgetPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Fill in the 'Email' field.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = null!;
    }
}

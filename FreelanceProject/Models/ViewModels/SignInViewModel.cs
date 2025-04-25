using FreelanceProject.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class SignInViewModel
    {
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Fill in the 'Email' field.")]
        [Display(Name = "Email")]
        [EmailFormCheck]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Fill in the 'Password' field.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }
}

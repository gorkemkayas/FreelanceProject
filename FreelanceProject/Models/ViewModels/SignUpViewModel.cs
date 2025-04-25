using FreelanceProject.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Fill in the 'Name' field.")]
        [Display(Name = "Name")]
        [StringCheck]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Fill in the 'Surname' field.")]
        [Display(Name = "Surname")]
        [StringCheck]
        public string Surname { get; set; } = null!;


        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Fill in the 'Email' field.")]
        [Display(Name = "Email")]
        [EmailFormCheck]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Fill in the 'Password' field.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = null!;

        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Required(ErrorMessage = "Fill in the 'Confirm Password' field.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = null!;

        [Required(ErrorMessage = "Fill in the 'Birthdate' field.")]
        [Display(Name = "Birtdate")]
        public DateTime BirthDate { get; set; }
    }
}

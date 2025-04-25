using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class SignUpViewModel
    {
        [Required(ErrorMessage = "Fill in the 'Name' field.")]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Fill in the 'Surname' field.")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }


        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Fill in the 'Email' field.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Fill in the 'Password' field.")]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        [Required(ErrorMessage = "Fill in the 'Confirm Password' field.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Fill in the 'Birthdate' field.")]
        [Display(Name = "Birtdate")]
        public DateTime BirthDate { get; set; }
    }
}

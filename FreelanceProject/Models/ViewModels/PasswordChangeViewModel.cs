using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class PasswordChangeViewModel
    {
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fill in the 'Old Password' field.")]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; } = null!;

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fill in the 'New Password' field.")]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = null!;

        [Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match.")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Fill in the 'Confirm New Password' field.")]
        [Display(Name = "Confirm New Password")]
        public string ConfirmNewPassword { get; set; } = null!;
    }
}

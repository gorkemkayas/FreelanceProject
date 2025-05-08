using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Models.ViewModels
{
    public class ExtendedProfileViewModel : VisitorProfileViewModel
    {
        public string Id { get; set; } = null!;
        public string? PhoneNumber { get; set; }
        public string? CVPath { get; set; }

        [Required]
        public string EmailAddress { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }

        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; }

        public void SetProperty(PhotoType photoType, string value)
        {
            switch (photoType)
            {
                case PhotoType.ProfilePicture:
                    ProfilePicture = value;
                    break;
                case PhotoType.CoverImagePicture:
                    CoverImagePicture = value;
                    break;
                case PhotoType.WorkingAtLogo:
                    WorkingAtLogo = value;
                    break;
                case PhotoType.CV:
                    CVPath = value;
                    break;
                default:
                    throw new Exception("Invalid operation.");
            }
        }

        public enum PhotoType
        {
            ProfilePicture = 0,
            CoverImagePicture = 1,
            WorkingAtLogo = 2,
            CV = 3
        }
    }
}

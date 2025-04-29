using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Utilities;
using Microsoft.AspNetCore.Identity;
using static FreelanceProject.Models.ViewModels.ExtendedProfileViewModel;

namespace FreelanceProject.Services.Abstract
{
    public interface IUserService
    {
        Task<ServiceResult<AppUser>> CreateUserAsync(SignUpViewModel request);
        Task<ServiceResult<AppUser>> SignInAsync(SignInViewModel request);
        Task<ServiceResult<AppUser>> UpdateProfileAsync(AppUser oldUserInfo, ExtendedProfileViewModel newUserInfo, IFormFile? fileInputProfile, IFormFile? coverInputProfile, IFormFile? IconInputWorkingAt);
        public Task<ExtendedProfileViewModel> ConfigurePictureAsync(ExtendedProfileViewModel newUserInfo, AppUser oldUserInfo, IFormFile? formFile, PhotoType type);

        Task<ServiceResult<AppUser>> ResetPasswordAsync(string newPassword, string userId, string token);

        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);

        Task<AppUser>? FindByEmailAsync(string email);
        Task SignOutAsync();

        bool CheckEmailConfirmed(AppUser user);
        Task<AppUser> FindByNameAsync(string userName);
        Task<int> GetCommentCountByUserAsync(AppUser user);
        Task<int> GetPostCountByUserAsync(AppUser user);
        ExtendedProfileViewModel GetExtendedProfileViewModel(AppUser user);
        VisitorProfileViewModel GetVisitorProfileViewModel(AppUser user);
        Task<ServiceResult<AppUser>> ConfirmEmailAsync(ConfirmEmailViewModel request);

    }
}

using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Utilities;

namespace FreelanceProject.Services.Abstract
{
    public interface IUserService
    {
        Task<ServiceResult<AppUser>> CreateUserAsync(SignUpViewModel request);
        Task<ServiceResult<AppUser>> SignInAsync(SignInViewModel request);

        Task<ServiceResult<AppUser>> ResetPasswordAsync(string newPassword, string userId, string token);

        Task<string> GeneratePasswordResetTokenAsync(AppUser user);
        Task<string> GenerateEmailConfirmationTokenAsync(AppUser user);

        Task<AppUser>? FindByEmailAsync(string email);
        Task SignOutAsync();

        bool CheckEmailConfirmed(AppUser user);
        Task<ServiceResult<AppUser>> ConfirmEmailAsync(ConfirmEmailViewModel request);

    }
}

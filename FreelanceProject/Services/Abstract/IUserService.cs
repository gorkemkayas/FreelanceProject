using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Utilities;

namespace FreelanceProject.Services.Abstract
{
    public interface IUserService
    {
        Task<ServiceResult<AppUser>> CreateUserAsync(SignUpViewModel request);
        Task<ServiceResult<AppUser>> SignInAsync(SignInViewModel request);
        Task SignOutAsync();

    }
}

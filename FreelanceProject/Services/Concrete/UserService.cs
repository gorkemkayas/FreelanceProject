using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Services.Abstract;
using FreelanceProject.Utilities;
using static FreelanceProject.CustomMethods.CustomMethods;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace FreelanceProject.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<ServiceResult<AppUser>> ConfirmEmailAsync(ConfirmEmailViewModel request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Token))
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError() { Code = "ModelEmpty", Description = "Request cannot be null" } }
                };
            }
            var user = await _userManager.FindByIdAsync(request.UserId);

            var result = await _userManager.ConfirmEmailAsync(user!, request.Token);
            if (!result.Succeeded)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = result.Errors.ToList()
                };
            }
            return new ServiceResult<AppUser>()
            {
                IsSuccess = true
            };
        }

        public async Task<ServiceResult<AppUser>> CreateUserAsync(SignUpViewModel request)
        {
            if (request is null)
            {
                var err = new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError() { Code = "ModelEmpty", Description = "Request cannot be null" } }
                };

                return err;
            }

            var result = await _userManager.CreateAsync(new AppUser()
            {
                UserName = GenerateUsername(request.Email),
                Email = request.Email,
                Name = request.Name,
                Surname = request.Surname,
                BirthDate = request.BirthDate
            }, request.Password);

            if (!result.Succeeded)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = result.Errors.ToList()
                };
            }

            return new ServiceResult<AppUser>()
            {
                IsSuccess = true,
                Data = await _userManager.FindByEmailAsync(request.Email)
            };
        }

        public async Task<AppUser>? FindByEmailAsync(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            return result;
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(AppUser user)
        {
            var token =  await _userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<ServiceResult<AppUser>> ResetPasswordAsync(string newPassword, string userId,string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError>() { new IdentityError() { Code = "UserNotFound", Description = "User not found" } }
                };
            }
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

            if(!result.Succeeded)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = result.Errors.ToList()
                };
            }
            return new ServiceResult<AppUser>()
            {
                IsSuccess = true
            };
        }
        public async Task<ServiceResult<AppUser>> SignInAsync(SignInViewModel request)
        {
            if(request is null)
            {
                var err = new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError() { Code = "ModelEmpty", Description = "Request cannot be null" } }
                };
                return err;
            }
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError() { Code = "UserNotFound", Description = "User not found" } }
                };
            }

            var loginProcess = await _signInManager.PasswordSignInAsync(user, request.Password, request.RememberMe, true);

            if(!loginProcess.Succeeded)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError() { Code = "LoginFailed", Description = $"Email or password incorrect. Your have {5 - user.AccessFailedCount} remaining attemps." } }
                };
            }

            return new ServiceResult<AppUser>()
            {
                IsSuccess = true
            };
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}

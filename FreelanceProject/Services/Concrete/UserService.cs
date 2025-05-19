using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Services.Abstract;
using FreelanceProject.Utilities;
using static FreelanceProject.CustomMethods.CustomMethods;
using Microsoft.AspNetCore.Identity;
using System.Net;
using FreelanceProject.Data.Context;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using static FreelanceProject.Models.ViewModels.ExtendedProfileViewModel;
using FreelanceProject.Areas.Admin.Models;

namespace FreelanceProject.Services.Concrete
{
    public class UserService : IUserService
    {
        private readonly FreelanceDbContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, FreelanceDbContext dbContext, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public bool CheckEmailConfirmed(AppUser user)
        {
            return user.EmailConfirmed;
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

        public async Task<int> GetCommentCountByUserAsync(AppUser user)
        {
            var commentCount = await _dbContext.Comments.Where(x => x.AuthorId == user.Id).CountAsync();
            return commentCount;
        }

        public async Task<int> GetPostCountByUserAsync(AppUser user)
        {
            var postCount = await _dbContext.Posts.Where(x => x.AuthorId == user.Id).CountAsync();
            return postCount;
        }

        public ExtendedProfileViewModel GetExtendedProfileViewModel(AppUser user)
        {
            var extendedProfile = _mapper.Map<ExtendedProfileViewModel>(user);

            var createdJobs = _dbContext.Jobs.Where(o => o.OwnerId == user.Id).ToList();
            var completedJobs = _dbContext.Jobs.Where(a => a.JobApplications.Any(b => b.ApplicantId == user.Id && b.Status == JobApplicationStatus.Completed)).ToList();

            extendedProfile.CreatedJobs = createdJobs;
            extendedProfile.CompletedJobs = completedJobs;
            return extendedProfile;
        }
        public VisitorProfileViewModel GetVisitorProfileViewModel(AppUser user)
        {
            var visitorProfile = _mapper.Map<VisitorProfileViewModel>(user);

            var createdJobs = _dbContext.Jobs.Where(o => o.OwnerId == user.Id).ToList();
            var completedJobs = _dbContext.Jobs.Where(a => a.JobApplications.Any(b => b.ApplicantId == user.Id && b.Status == JobApplicationStatus.Completed)).ToList();

            visitorProfile.CreatedJobs = createdJobs;
            visitorProfile.CompletedJobs = completedJobs;

            return visitorProfile;
        }
        public async Task<ExtendedProfileViewModel> ConfigurePictureAsync(ExtendedProfileViewModel newUserInfo, AppUser oldUserInfo, IFormFile? formFile, PhotoType type)
        {

            if (formFile is null)
            {
                newUserInfo.SetProperty(type, oldUserInfo.GetPropertyValue(type));
                return newUserInfo;
            }

            string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", "userPhotos", $"{oldUserInfo.UserName}");
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }
            var fileName = formFile.FileName.Replace(" ", "_");
            string filePath = Path.Combine(uploadPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }
            newUserInfo.SetProperty(type, fileName);

            if (oldUserInfo.GetPropertyValue(type) != null)
            {
                var oldPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), uploadPath, oldUserInfo.GetPropertyValue(type));

                if (File.Exists(oldPhotoPath))
                {
                    File.Delete(oldPhotoPath);
                }

            }
            return newUserInfo;
        }
        public async Task<ServiceResult<AppUser>> UpdateProfileAsync(AppUser oldUserInfo, ExtendedProfileViewModel newUserInfo, IFormFile? fileInputProfile, IFormFile? coverInputProfile, IFormFile? IconInputWorkingAt, IFormFile? CVFile)
        {
            var errors = new List<IdentityError>();

            bool criticalUpdate = false;

            if (oldUserInfo == null)
            {
                errors.Add(new() { Code = "UserNotFound", Description = "The user not found in the system." });
                return new ServiceResult<AppUser>() { IsSuccess = false, Errors = errors, isCritical = false };
            }

            if (oldUserInfo.Id.ToString() != newUserInfo.Id)
            {
                errors.Add(new() { Code = "UsersNotMatched", Description = "The users not matched." });
                return new ServiceResult<AppUser>() { IsSuccess = false, Errors = errors, isCritical = false };
            }

            if (oldUserInfo.Email != newUserInfo.EmailAddress) criticalUpdate = true;

            var step1 = await ConfigurePictureAsync(newUserInfo, oldUserInfo, fileInputProfile, PhotoType.ProfilePicture);
            var step2 = await ConfigurePictureAsync(step1, oldUserInfo, coverInputProfile, PhotoType.CoverImagePicture);
            var step3 = await ConfigurePictureAsync(step2, oldUserInfo, IconInputWorkingAt, PhotoType.WorkingAtLogo);
            var step4 = await ConfigurePictureAsync(step2, oldUserInfo, CVFile, PhotoType.CV);


            //var step1 = await ConfigureProfilePictureOfNewUserInfoAsync(newUserInfo, oldUserInfo, fileInputProfile);
            //var step2 = await ConfigureCoverPictureOfNewUserInfoAsync(step1, oldUserInfo, coverInputProfile);
            //var step3 = await ConfigureWorkingIconOfNewUserInfoAsync(step2, oldUserInfo, IconInputWorkingAt);

            var updatedUser = _mapper.Map(step3, oldUserInfo);
            await _userManager.UpdateAsync(updatedUser);

            if (criticalUpdate)
            {
                await _userManager.UpdateSecurityStampAsync(oldUserInfo);
                return new ServiceResult<AppUser>() { IsSuccess = true, isCritical = true };
            }

            return new ServiceResult<AppUser>() { IsSuccess = true ,isCritical = false };
        }


        public async Task<AppUser> FindByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user is null)
            {
                throw new Exception("User not found");
            }
            return user;
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
            var checkEmailConfirmed = CheckEmailConfirmed(user);

            if(!checkEmailConfirmed)
            {
                return new ServiceResult<AppUser>()
                {
                    IsSuccess = false,
                    Errors = new List<IdentityError> { new IdentityError() { Code = "EmailNotConfirmed", Description = "Email not confirmed" } }
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

        public async Task<ItemPagination<UserViewModel>> GetPagedUsersAsync(int page, int pageSize, bool includeDeleted = false)
        {
            var itemsQuery = _userManager.Users;
            if (!includeDeleted)
            {
                itemsQuery = itemsQuery.Where(p => p.IsDeleted == false);
            }

            var pagedUsers = new ItemPagination<UserViewModel>()
            {
                PageSize = pageSize,
                CurrentPage = page,
                TotalCount = (includeDeleted is true) ? _userManager.Users.Count() : _userManager.Users.Where(p => p.IsDeleted == false).Count(),
                Items = await itemsQuery
                                    .Skip((page - 1) * pageSize)
                                    .Take(pageSize)
                                    .Select(user => new UserViewModel
                                    {
                                        Id = user.Id,
                                        Name = user.Name!,
                                        Surname = user.Surname!,
                                        Username = user.UserName,
                                        IsDeleted = user.IsDeleted,
                                        Email = user.Email,
                                        PhoneNumber = user.PhoneNumber
                                    }).ToListAsync()
            };

            return pagedUsers;
        }
    }
}

using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Utilities;
using System.Reflection;

namespace FreelanceProject.Services.Abstract
{
    public interface IJobService
    {
        Task<string> ConfigureJobImage(AppUser user, IFormFile jobImage, string jobTitle);
        Task<ServiceResult<JobEntity>> AddNewJobAsync(AppUser user, CreateJobViewModel request);
        Task<ServiceResult<JobEntity>> EditJobAsync(AppUser user, EditJobViewModel request);
    }
}

using FreelanceProject.Data.Context;
using FreelanceProject.Data.Entities;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Services.Abstract;
using FreelanceProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FreelanceProject.Services.Concrete
{
    public class JobService : IJobService
    {
        private readonly FreelanceDbContext _dbContext;

        public JobService(FreelanceDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<string> ConfigureJobImage(AppUser user, IFormFile jobImage, string jobTitle)
        {
            // Dosya adı ve uzantısı
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(jobImage.FileName);

            // wwwroot yolunu al
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var JobTitle = jobTitle.Replace(" ", "-");

            // Hedef klasör
            var uploadPath = Path.Combine(wwwRootPath, "Users",$"{user.UserName}", "CreatedJobs", $"{jobTitle}");

            // Klasör yoksa oluştur
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            // Tam dosya yolu
            var filePath = Path.Combine(uploadPath, fileName);

            // Dosyayı kaydet
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await jobImage.CopyToAsync(stream);
            }

            var fileUrl = $"/Users/{user.UserName}/CreatedJobs/{jobTitle}/{fileName}";
            return fileUrl;
        }

        public async Task<ServiceResult<JobEntity>> AddNewJobAsync(AppUser user, CreateJobViewModel request)
        {
            int calcUnit = 0;
            switch (request.DurationUnit)
            {
                case "Gün":
                    calcUnit = 1;
                    break;
                case "Hafta":
                    calcUnit = 7;
                    break;
                case "Ay":
                    calcUnit = 30;
                    break;
            }
            var deadLine = request.Duration * calcUnit;

            var result = await _dbContext.AddAsync(new JobEntity()
            {
                Title = request.Title,
                Description = request.Description,
                Requirements = request.Requirements,
                Budget = request.Budget,
                StartDate = request.StartDate,
                Deadline = request.StartDate.AddDays(deadLine),
                Category = request.Category,
                JobImage = await ConfigureJobImage(user, request.ImageFile, request.Title),
                OwnerId = user.Id
            });

            if (result == null)
            {
                return new ServiceResult<JobEntity>()
                {
                    IsSuccess = false,
                    Errors = { new IdentityError() { Code = "JobCannotBecCreated", Description = "Job oluşturulurken hata oluştu." } }
                };
            }

            await _dbContext.SaveChangesAsync();
            return new ServiceResult<JobEntity>() { IsSuccess = true };
        }

        public async Task<ServiceResult<JobEntity>> EditJobAsync(AppUser user, EditJobViewModel request)
        {
            int calcUnit = 0;
            switch (request.DurationUnit)
            {
                case "Gün":
                    calcUnit = 1;
                    break;
                case "Hafta":
                    calcUnit = 7;
                    break;
                case "Ay":
                    calcUnit = 30;
                    break;
            }
            var deadLine = request.Duration * calcUnit;
            var job = await _dbContext.Jobs.FindAsync(request.Id);

            if(job is null)
            {
                return new ServiceResult<JobEntity>()
                {
                    IsSuccess = false,
                    Errors = { new IdentityError() { Code = "JobNotFound", Description = "Job bulunamadı." } }
                };
            }
            if(job.Title != request.Title)
            {
                var result = CustomMethods.CustomMethods.ChangeFolderName(user, job.Title, request.Title);
            }
            job.Title = request.Title;
            job.Description = request.Description;
            job.Requirements = request.Requirements;
            job.Budget = request.Budget;
            job.Deadline = request.StartDate.AddDays(deadLine);
            job.Category = request.Category;
            job.ModifiedDate = DateTime.Now;
            if(request.ImageFile != null) job.JobImage = await ConfigureJobImage(user, request.ImageFile, request.Title);
            

            await _dbContext.SaveChangesAsync();
            return new ServiceResult<JobEntity>() { IsSuccess = true };
        }
    }
}

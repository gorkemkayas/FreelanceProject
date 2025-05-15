using FreelanceProject.Data.Context;
using FreelanceProject.Data.Entities;
using FreelanceProject.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FreelanceProject.Controllers
{
    public class FreelancerController : Controller
    {
        private readonly FreelanceDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FreelancerController(FreelanceDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyApplications(JobApplicationStatus status)
        {
            var jobs = await _context.JobApplications.Where(x => x.ApplicantId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.Status == status)
                .Include(x => x.Applicant)
                .Include(n => n.Job).ThenInclude(o => o.Owner).ToListAsync();

            if(jobs is null)
            {
                var data2 = new JobsAndCurrentTab { Jobs = new(), CurrentTab = status.ToString() };
                return View(data2);
            }
            var data = new  JobsAndCurrentTab{Jobs = jobs, CurrentTab = status.ToString() };
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitProject(IFormFile projectFile, string projectNotes, Guid jobId)
        {
            if (projectFile == null || projectFile.Length == 0)
            {
                ModelState.AddModelError("projectFile", "Lütfen bir dosya seçin.");
                return View(); // veya hata mesajıyla birlikte geri dön
            }

            // İzin verilen dosya uzantıları
            var allowedExtensions = new[] { ".zip", ".rar", ".pdf", ".docx", ".pptx" };
            var fileExtension = Path.GetExtension(projectFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                ModelState.AddModelError("projectFile", "Geçersiz dosya formatı. İzin verilen formatlar: " + string.Join(", ", allowedExtensions));
                return View();
            }

            var currentUser = await _context.Users.FirstOrDefaultAsync(x => x.Id.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentJob = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);

            // Dosyayı kaydetme
            //"/Users/@applicant.UserName/AppliedJobs/@applicant.JobTitle-@applicant.JobId";
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", $"{currentUser.UserName}","AppliedJobs",$"{currentJob.Title}-{currentJob.Id.ToString().Substring(0,5)}");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + projectFile.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await projectFile.CopyToAsync(fileStream);
            }

            var jobApp = await _context.JobApplications.Where(a => a.Job.Id == jobId && a.ApplicantId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier)).FirstOrDefaultAsync();

            jobApp!.ProjectFile = uniqueFileName;

            await _context.SaveChangesAsync();

            // Veritabanına kayıt işlemleri burada yapılabilir
            // ...

            TempData["SuccessMessage"] = "Projeniz başarıyla teslim edildi.";
            return RedirectToAction("MyApplications","Freelancer"); // veya başka bir sayfaya yönlendirme
        }

        public async Task<IActionResult> Download(string fileName, Guid jobId,Guid appliciantId)
        {
            var applicantUser = await _context.Users.FindAsync(appliciantId);
            var currentJob = await _context.Jobs.FirstOrDefaultAsync(x => x.Id == jobId);
            // Dosya yolu
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Users", $"{applicantUser!.UserName}", "AppliedJobs", $"{currentJob.Title}-{currentJob.Id.ToString().Substring(0, 5)}",$"{fileName}");

            // Dosyanın varlığını kontrol et
            if (!System.IO.File.Exists(filePath))
                return NotFound("Dosya bulunamadı.");

            // Dosya uzantısına göre içerik tipi belirle
            string contentType = fileName switch
            {
                var name when name.EndsWith(".pdf") => "application/pdf",
                var name when name.EndsWith(".pptx") => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
                var name when name.EndsWith(".rar") => "application/x-rar-compressed",
                var name when name.EndsWith(".zip") => "application/zip",
                var name when name.EndsWith(".txt") => "text/plain",
                _ => "application/octet-stream" // Diğer dosya türleri
            };

            // Dosyayı indirmek için döndür
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, contentType, fileName);
        }
    }
}

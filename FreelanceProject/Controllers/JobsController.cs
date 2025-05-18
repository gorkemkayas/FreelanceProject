using FreelanceProject.Data.Entities;    // Entity dosyasını da ekle
using FreelanceProject.Extensions;
using FreelanceProject.Models.ViewModels;  // ViewModel dosyasını eklemeyi unutma
using FreelanceProject.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FreelanceProject.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;       // DbContext dosyasını da ekle
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Security.Claims;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.IdentityModel.Tokens;

namespace FreelanceProject.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly FreelanceDbContext _context; // DbContext
        private readonly UserManager<AppUser> _userManager;
        private readonly IJobService _jobService;
        private readonly IMapper _mapper;


        public JobsController(FreelanceDbContext context, UserManager<AppUser> userManager, IJobService jobService, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _jobService = jobService;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Details(Guid id)// iş detaylarını görüntüle.
        {


            //return View(job); // Bulunan iş ilanını view'a gönderir.
            //var job = _context.Jobs.FirstOrDefault(j => j.Id == id);
            var job = await _context.Jobs
      .Include(x => x.Owner)
      .FirstOrDefaultAsync(j => j.Id == id);

            if (job == null)
            {
                return NotFound(); // Eğer iş ilanı bulunamazsa 404 sayfası döner.
            }

            // Sorguya göre filtrelenmiş job listesi
            var jobsQuery = _context.Jobs.Include(e => e.Owner).AsQueryable();
            // Son 3 iş ilanı
            ViewBag.LastThreeJobs = jobsQuery
                .OrderByDescending(j => j.Id)
                .Take(3)
                .ToList();

            // Aynı kategoriye sahip benzer işler
            ViewBag.SimilarJobs = _context.Jobs
                .Where(j => j.Category == job.Category && j.Id != job.Id)
                .OrderByDescending(j => j.Id)
                .Take(3)
                .ToList();

            var isApplied  = _context.JobApplications.Any(ap => ap.Job == job && ap.ApplicantId.ToString() == User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var model = new JobAndApplicationExistsViewModel()
            {
                Job = job,
                Applied = isApplied

            };
            return View(model); // Ana model sadece JobEntity nesnesi

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApplyForJob(Guid jobId)
        {
            var user = await _userManager.GetUserAsync(User);
            var selectedJob = await _context.Jobs.FindAsync(jobId);
            if (user == null)
            {
                return Unauthorized();
            }
            if (selectedJob == null)
            {
                return Unauthorized();
            }

            // Kullanıcı CV bilgisi kontrolü
            if (string.IsNullOrEmpty(user.CVPath))  // CV bilgisi boşsa
            {
                //TempData["ErrorMessage"] = "CV'nizi doldurmanız gerekmektedir. Lütfen profilinizi güncelleyiniz.";
                //// Kullanıcının profil sayfasına yönlendir
                //return RedirectToAction("Profile", "User", new { userName = user.UserName });

                TempData["ErrorMessage"] = "CV’nizi doldurmanız gerekmektedir. Lütfen profilinizi güncelleyiniz.";
                return RedirectToAction("Details", new { id = jobId });

            }
            if (user.Id == selectedJob.OwnerId)  // CV bilgisi boşsa
            {
                //TempData["ErrorMessage"] = "CV'nizi doldurmanız gerekmektedir. Lütfen profilinizi güncelleyiniz.";
                //// Kullanıcının profil sayfasına yönlendir
                //return RedirectToAction("Profile", "User", new { userName = user.UserName });

                TempData["OwnJobError"] = "Kendi oluşturduğunuz işe başvuramazsınız!";
                return RedirectToAction("Details", new { id = jobId });

            }


            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound();
            }

            bool exists = await _context.JobApplications.AnyAsync(n => n.ApplicantId == user.Id && n.JobId == jobId);
            if (exists)
            {
                TempData["AlreadyApplied"] = "Bu işe birden fazla kez başvuramazsını.";
                return RedirectToAction("Details", new { id = jobId });
            }

            var jobApplication = new JobApplicationEntity
            {
                JobId = jobId,
                ApplicantId = user.Id,
                Status = JobApplicationStatus.Pending,
                AppliedDate = DateTime.UtcNow,
                IsApprovedByEmployer = false,
                IsApprovedByApplicant = false
            };

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            // Başvuru işlemi tamamlandıktan sonra MyApplications sayfasına yönlendir
            return RedirectToAction("MyApplications", "Freelancer");
        }


        [HttpGet]
        public IActionResult Create() // Yeni iş ilanı  oluşturma
        {
            var categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };  // Sabit Kategoriler
            ViewBag.Categories = categories; // Kategorileri View'a gönder

            return View();
        }


        [Route("Messages")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateJobViewModel model)
        {
            ViewBag.Categories = new List<string>
            {
                     "Web Development", "Mobile Development", "Design", "SEO", "Marketing"
            };

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Model hatalı!");
                return View(model);
            }

            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var result = await _jobService.AddNewJobAsync(currentUser!, model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors.ToList());
                return View(model);
            }
            return RedirectToAction("EmployerJobs", "Jobs");
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            ViewBag.Categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };

            var job = await _context.Jobs.FindAsync(id);
            var mappedJob = _mapper.Map<EditJobViewModel>(job);

            return View(mappedJob);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditJobViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Kategori listesi eksik olursa sayfa hata verir
                ViewBag.Categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };
                return View(model);
            }
            var currentUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            var result = await _jobService.EditJobAsync(currentUser!, model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors.ToList());
                return View();
            }

            return RedirectToAction(nameof(EmployerJobs));
        }


        // GET: Jobs/Delete/{id}
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var job = await _context.Jobs
                .FirstOrDefaultAsync(m => m.Id == id);

            //if (job == null)
            //{
            //    return NotFound();
            //}

            if (job == null || job.IsDeleted || !job.IsActive)
            {
                TempData["ErrorMessage"] = "Bu ilan zaten silinmiş veya aktif değil.";
                return RedirectToAction("EmployerJobs");
            }

            return View(job);
        }



        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            //if (job == null)
            //{
            //    return NotFound();
            //}

            if (job == null || job.IsDeleted || !job.IsActive)
            {
                TempData["ErrorMessage"] = "İlan zaten silinmiş.";
                return RedirectToAction("EmployerJobs");
            }

            // Silme yerine işaretleme işlemi
            job.IsDeleted = true;
            job.IsActive = false;
            job.DeletedDate = DateTime.Now;

            _context.Jobs.Update(job);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(EmployerJobs));
        }




        [ResponseCache(Duration = 0, NoStore = true)]
        public async Task<IActionResult> EmployerJobs()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // İşverenin iş ilanları
            var jobs = await _context.Jobs
                .Where(j => j.OwnerId == user.Id) // İlan sahibi olan işverenin ilanları
                .OrderByDescending(j => j.CreatedDate)
                .Include(j => j.JobApplications) // Başvuruları da dahil et
                .ThenInclude(ja => ja.Applicant) // Başvuran kullanıcı bilgilerini de dahil et
                .ToListAsync();

            return View(jobs);
        }


        public async Task<IActionResult> ViewApplicants(Guid jobId)
        {
            var job = await _context.Jobs
                .Include(j => j.JobApplications)
                    .ThenInclude(ja => ja.Applicant)
                .FirstOrDefaultAsync(j => j.Id == jobId);

            if (job == null)
            {
                return NotFound();
            }

            // Giriş yapan kişinin bu ilana sahip olup olmadığını kontrol et
            var currentUser = await _userManager.GetUserAsync(User);
            if (job.OwnerId != currentUser.Id)
            {
                return Forbid(); // Başkasının ilanına erişemezsin
            }

            ViewBag.JobId = jobId;

            // Başvuranları ApplicantViewModel formatında döndür
            var applicants = job.JobApplications.Select(ja => new ApplicantViewModel
            {
                Id =ja.Id,
                ApplicantId = ja.ApplicantId,
                UserName = ja.Applicant.UserName,
                FullName = ja.Applicant.FullName,
                ProfilePicture = ja.Applicant.ProfilePicture,
                Status = ja.Status,
                IsApprovedByEmployer = ja.IsApprovedByEmployer,
                IsApprovedByApplicant = ja.IsApprovedByApplicant,
                Email = ja.Applicant.Email,
                CVPath = ja.Applicant.CVPath,
                JobTitle = ja.Job.Title,
                JobId = ja.Job.Id,
                FileName = ja.ProjectFile


            }).ToList();

            return View(applicants);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateApplicationStatus(Guid userId, Guid jobId, bool approve)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.ApplicantId == userId && a.JobId == jobId);

            if (application == null)
            {
                TempData["ErrorMessage"] = "İşlem başarısız: Geçersiz kullanıcı veya iş ID.";
                return RedirectToAction("ViewApplicants", new { jobId = jobId });
            }

            if (approve)
            {
                // Eğer onay veriliyorsa, diğer başvuruları reddediyoruz
                var otherApplications = await _context.JobApplications
                    .Where(a => a.JobId == jobId && a.ApplicantId != userId)
                    .ToListAsync();

                foreach (var otherApplication in otherApplications)
                {
                    otherApplication.Status = JobApplicationStatus.Rejected;
                    otherApplication.IsApprovedByEmployer = false; // Diğer başvuruları reddet
                }
            }

            application.IsApprovedByEmployer = approve;
            application.Status = approve ? JobApplicationStatus.Ongoing : JobApplicationStatus.Rejected;

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = approve ? "Başvuru onaylandı." : "Başvuru reddedildi.";
            return RedirectToAction("ViewApplicants", new { jobId = jobId });
        }

        [HttpPost]
        public async Task<IActionResult> CompleteApplicationStatus(Guid userId, Guid jobId, bool approve,JobApplicationStatus status)
        {
            var application = await _context.JobApplications
                .FirstOrDefaultAsync(a => a.ApplicantId == userId && a.JobId == jobId);

            if (application == null)
            {
                TempData["ErrorMessage"] = "İşlem başarısız: Geçersiz kullanıcı veya iş ID.";
                return RedirectToAction("ViewApplicants", new { jobId = jobId });
            }

            //if (approve)
            //{
            //    // Eğer onay veriliyorsa, diğer başvuruları reddediyoruz
            //    var otherApplications = await _context.JobApplications
            //        .Where(a => a.JobId == jobId && a.ApplicantId != userId)
            //        .ToListAsync();

            //    foreach (var otherApplication in otherApplications)
            //    {
            //        otherApplication.Status = JobApplicationStatus.Rejected;
            //        otherApplication.IsApprovedByEmployer = false; // Diğer başvuruları reddet
            //    }
            //}


            application.IsApprovedByEmployer = approve;
            application.Status = approve ? JobApplicationStatus.Completed : (application.Status == JobApplicationStatus.Ongoing ? JobApplicationStatus.Revise : JobApplicationStatus.Rejected);

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = approve ? "Başvuru onaylandı." : "Başvuru reddedildi.";
            return RedirectToAction("ViewApplicants", new { jobId = jobId });
        }

    }
}

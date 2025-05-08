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
        public IActionResult Details(Guid id) // iş detaylarını görüntüle.
        {
            //var job = _context.Jobs.FirstOrDefault(j => j.Id == id);

            //if (job == null)
            //{
            //    return NotFound(); // Eğer iş ilanı bulunamazsa 404 sayfası döner.
            //}

            //return View(job); // Bulunan iş ilanını view'a gönderir.
            var job = _context.Jobs.FirstOrDefault(j => j.Id == id);

            if (job == null)
            {
                return NotFound(); // Eğer iş ilanı bulunamazsa 404 sayfası döner.
            }

            // Sorguya göre filtrelenmiş job listesi
            var jobsQuery = _context.Jobs.AsQueryable();
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

            return View(job); // Ana model sadece JobEntity nesnesi

        }

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> ApplyForJob(Guid jobId)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var job = await _context.Jobs.FindAsync(jobId);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    var jobApplication = new JobApplicationEntity
        //    {
        //        JobId = jobId,
        //        ApplicantId = user.Id,
        //        Status = JobApplicationStatus.Pending,
        //        AppliedDate = DateTime.UtcNow,
        //        IsApprovedByEmployer = false,
        //        IsApprovedByApplicant = false
        //    };

        //    _context.JobApplications.Add(jobApplication);
        //    await _context.SaveChangesAsync();

        //    return RedirectToAction("ApplicantDetails", new { applicationId = jobApplication.Id });
        //}


        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> ApplyForJob(Guid jobId)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    var job = await _context.Jobs.FindAsync(jobId);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    var jobApplication = new JobApplicationEntity
        //    {
        //        JobId = jobId,
        //        ApplicantId = user.Id,
        //        Status = JobApplicationStatus.Pending,
        //        AppliedDate = DateTime.UtcNow,
        //        IsApprovedByEmployer = false,
        //        IsApprovedByApplicant = false
        //    };

        //    _context.JobApplications.Add(jobApplication);
        //    await _context.SaveChangesAsync();

        //    // Apply işlemi tamamlandıktan sonra MyApplications sayfasına yönlendirme yap
        //    return RedirectToAction("MyApplications", "Freelancer");
        //}

        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> ApplyForJob(Guid jobId)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    // Kullanıcı CV bilgisi kontrolü
        //    if (string.IsNullOrEmpty(user.CVUrl))  // CV bilgisi boşsa
        //    {
        //        TempData["ErrorMessage"] = "CV'nizi doldurmanız gerekmektedir. Lütfen profilinizi güncelleyiniz.";
        //        return RedirectToAction("User", "Profile");  // CV'yi güncellemesi için profil düzenleme sayfasına yönlendir
        //    }

        //    var job = await _context.Jobs.FindAsync(jobId);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    var jobApplication = new JobApplicationEntity
        //    {
        //        JobId = jobId,
        //        ApplicantId = user.Id,
        //        Status = JobApplicationStatus.Pending,
        //        AppliedDate = DateTime.UtcNow,
        //        IsApprovedByEmployer = false,
        //        IsApprovedByApplicant = false
        //    };

        //    _context.JobApplications.Add(jobApplication);
        //    await _context.SaveChangesAsync();

        //    // Başvuru işlemi tamamlandıktan sonra MyApplications sayfasına yönlendir
        //    return RedirectToAction("MyApplications", "Freelancer");
        //}


        //[HttpPost]
        //[Authorize]
        //public async Task<IActionResult> ApplyForJob(Guid jobId)
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        return Unauthorized();
        //    }

        //    // Kullanıcı CV bilgisi kontrolü
        //    if (string.IsNullOrEmpty(user.CVUrl))  // CV bilgisi boşsa
        //    {
        //        TempData["ErrorMessage"] = "CV'nizi doldurmanız gerekmektedir. Lütfen profilinizi güncelleyiniz.";
        //        // Kullanıcının profil sayfasına yönlendir
        //        return RedirectToAction("Profile", "User", new { id = user.Id });
        //    }

        //    var job = await _context.Jobs.FindAsync(jobId);
        //    if (job == null)
        //    {
        //        return NotFound();
        //    }

        //    var jobApplication = new JobApplicationEntity
        //    {
        //        JobId = jobId,
        //        ApplicantId = user.Id,
        //        Status = JobApplicationStatus.Pending,
        //        AppliedDate = DateTime.UtcNow,
        //        IsApprovedByEmployer = false,
        //        IsApprovedByApplicant = false
        //    };

        //    _context.JobApplications.Add(jobApplication);
        //    await _context.SaveChangesAsync();

        //    // Başvuru işlemi tamamlandıktan sonra MyApplications sayfasına yönlendir
        //    return RedirectToAction("MyApplications", "Freelancer");
        //}

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ApplyForJob(Guid jobId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            // Kullanıcı CV bilgisi kontrolü
            if (string.IsNullOrEmpty(user.CVUrl))  // CV bilgisi boşsa
            {
                //TempData["ErrorMessage"] = "CV'nizi doldurmanız gerekmektedir. Lütfen profilinizi güncelleyiniz.";
                //// Kullanıcının profil sayfasına yönlendir
                //return RedirectToAction("Profile", "User", new { userName = user.UserName });

                TempData["ErrorMessage"] = "CV’nizi doldurmanız gerekmektedir. Lütfen profilinizi güncelleyiniz.";
                return RedirectToAction("Details", new { id = jobId });

            }

            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound();
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



        public IActionResult Create() // Yeni iş ilanı  oluşturma
        {
            var categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };  // Sabit Kategoriler
            ViewBag.Categories = categories; // Kategorileri View'a gönder

            return View();
        }


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
        //public async Task<IActionResult> EmployerJobs()
        //{
        //    var user = await _userManager.GetUserAsync(User);
        //    if (user == null)
        //        return Unauthorized();

        //    // Burada Jobs tablosunu güncel verilerle çektiğinden emin ol
        //    var jobs = await _context.Jobs
        //        .Where(j => j.OwnerId == user.Id)
        //        .OrderByDescending(j => j.CreatedDate) // Veya ModifiedDate ile sıralayabilirsiniz
        //        .ToListAsync();

        //    return View(jobs);
        //}
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
    }
}

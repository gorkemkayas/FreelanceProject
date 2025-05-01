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

namespace FreelanceProject.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly FreelanceDbContext _context; // DbContext
        private readonly UserManager<AppUser> _userManager;

        public JobsController(FreelanceDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details() // iş detaylarını görüntüle.
        {
            return View();

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
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var job = new JobEntity
            {
                JobTitle = model.JobTitle,
                JobDescription = model.JobDescription,
                JobBudget = model.JobBudget,
                JobDuration = model.JobDuration,
                DurationUnit = model.DurationUnit,
                StartDate = model.StartDate,
                Category = model.Category, // Kategori bilgisini ekliyoruz
                EmployerId = user.Id,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            _context.Jobs.Add(job);
            await _context.SaveChangesAsync();

            return RedirectToAction("EmployerJobs", "Jobs");
        }



        public async Task<IActionResult> Edit(Guid id)
        {
            var job = await _context.Jobs.FindAsync(id);
            if (job == null)
            {
                return NotFound();
            }

            var model = new CreateJobViewModel
            {
                JobId = job.Id,
                JobTitle = job.JobTitle,
                JobDescription = job.JobDescription,
                JobBudget = job.JobBudget,
                JobDuration = job.JobDuration,
                DurationUnit = job.DurationUnit,
                StartDate = job.StartDate,
                Category = job.Category
            };

            // HATA BUNDAN DOLAYI GELİYORSA BU KISMI EKLEMEN GEREKİYOR
            var categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };
            ViewBag.Categories = categories;

            return View(model);
        }

        // İş ilanını düzenleme işlemi
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = await _context.Jobs.FindAsync(model.JobId);
                if (job == null)
                {
                    return NotFound();
                }

                job.JobTitle = model.JobTitle;
                job.JobDescription = model.JobDescription;
                job.JobBudget = model.JobBudget;
                job.JobDuration = model.JobDuration;
                job.DurationUnit = model.DurationUnit;
                job.StartDate = model.StartDate;
                job.Category = model.Category;
                job.ModifiedDate = DateTime.UtcNow;

                _context.Update(job);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(EmployerJobs));  // Listeye yönlendiriyoruz.
            }

            return View(model);
        }

        public IActionResult Delete()  // İş ilanını  silme
        {
            return View();
        }


        [ResponseCache(Duration = 0, NoStore = true)]
        public async Task<IActionResult> EmployerJobs()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            // Burada Jobs tablosunu güncel verilerle çektiğinden emin ol
            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == user.Id)
                .OrderByDescending(j => j.CreatedDate) // Veya ModifiedDate ile sıralayabilirsiniz
                .ToListAsync();

            return View(jobs);
        }
    }
}

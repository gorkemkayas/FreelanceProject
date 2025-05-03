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

namespace FreelanceProject.Controllers
{
    [Authorize]
    public class JobsController : Controller
    {
        private readonly FreelanceDbContext _context; // DbContext
        private readonly UserManager<AppUser> _userManager;
        private readonly IWebHostEnvironment _env;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public JobsController(FreelanceDbContext context, UserManager<AppUser> userManager,  IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            // _env = env;
            //IWebHostEnvironment env,
            _webHostEnvironment = webHostEnvironment;
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
            {
                ViewBag.Categories = new List<string>
                {
                     "Web Development", "Mobile Development", "Design", "SEO", "Marketing"
                 };
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            string imagePath = null;

            // Eğer dosya seçilmişse
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                // Dosya kaydedileceği dizini belirleyelim (wwwroot içinde uploads klasörü)
                var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");

                // Eğer uploads klasörü yoksa, oluşturuyoruz
                if (!Directory.Exists(uploadDir))
                    Directory.CreateDirectory(uploadDir);

                // Dosya adını benzersiz yapmak için GUID kullanıyoruz
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);

                // Dosyanın tam yolu
                var fullPath = Path.Combine(uploadDir, fileName);

                // Dosyayı belirtilen klasöre kaydediyoruz
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                // Kaydedilen dosyanın web yolu
                imagePath = "/uploads/" + fileName;
            }

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
                IsActive = true,
                ImageUrl = imagePath // Resim URL'sini veya dosya yolunu ekliyoruz
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
                Category = job.Category,
                ImageUrl = job.ImageUrl
            };

            // HATA BUNDAN DOLAYI GELİYORSA BU KISMI EKLEMEN GEREKİYOR
            var categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };
            ViewBag.Categories = categories;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateJobViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Kategori listesi eksik olursa sayfa hata verir
                ViewBag.Categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };
                return View(model);
            }

            if (ModelState.IsValid)
            {

                var job = await _context.Jobs.FindAsync(model.JobId);
                if (job == null)
                {
                    return NotFound();
                }

                // Resim güncellenmişse
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadDir))
                        Directory.CreateDirectory(uploadDir);

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                    var fullPath = Path.Combine(uploadDir, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);
                    }

                    job.ImageUrl = "/uploads/" + fileName; // Yeni resimle değiştiriyoruz
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

                return RedirectToAction(nameof(EmployerJobs));
            }

            // ❗ ModelState geçerli değilse buraya düşer → Kategoriler yeniden ViewBag'e eklenmeli!
            var categories = new List<string> { "Web Development", "Mobile Development", "Design", "SEO", "Marketing" };
            ViewBag.Categories = categories;

            return View(model); // ViewBag olmadan dönerse kategori listesi görünmez.
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

            // Burada Jobs tablosunu güncel verilerle çektiğinden emin ol
            var jobs = await _context.Jobs
                .Where(j => j.EmployerId == user.Id)
                .OrderByDescending(j => j.CreatedDate) // Veya ModifiedDate ile sıralayabilirsiniz
                .ToListAsync();

            return View(jobs);
        }
    }
}

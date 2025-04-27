using FreelanceProject.Data.Entities;    // Entity dosyasını da ekle
using FreelanceProject.Extensions;
using FreelanceProject.Models.ViewModels;  // ViewModel dosyasını eklemeyi unutma
using FreelanceProject.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using FreelanceProject.Data.Context;       // DbContext dosyasını da ekle

namespace FreelanceProject.Controllers
{
    public class JobsController : Controller
    {
        private readonly FreelanceDbContext _context; // DbContext

        public JobsController(FreelanceDbContext context)
        {
            _context = context;
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateJobViewModel model)
        {
            if (ModelState.IsValid)
            {
                var job = new JobEntity
                {
                    JobTitle = model.JobTitle,
                    JobDescription = model.JobDescription,
                    JobBudget = model.JobBudget,
                    JobDuration = model.JobDuration,
                    DurationUnit = model.DurationUnit,
                    StartDate = model.StartDate,
                    Skills = model.Skills != null ? string.Join(",", model.Skills) : null
                };

                _context.Jobs.Add(job);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

        public IActionResult Edit()  // İş ilanını düzenleme
        {
             return View();
        }
        public IActionResult Delete()  // İş ilanını  silme
        {
            return View();
        }

        public IActionResult EmployerJobs()  
        {
            return View();
        }
    }
}

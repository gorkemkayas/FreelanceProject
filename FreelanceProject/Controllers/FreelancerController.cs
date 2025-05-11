using FreelanceProject.Data.Context;
using FreelanceProject.Data.Entities;
using FreelanceProject.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FreelanceProject.Controllers
{
    public class FreelancerController : Controller
    {
        private readonly FreelanceDbContext _context;

        public FreelancerController(FreelanceDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyApplications(JobApplicationStatus status)
        {
            var jobs = await _context.JobApplications.Where(x => x.ApplicantId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier) && x.Status == status)
                .Include(x => x.Applicant)
                .Include(n => n.Job).ToListAsync();
            var data = new  JobsAndCurrentTab{Jobs = jobs, CurrentTab = status.ToString() };
            return View(data);
        }
    }
}

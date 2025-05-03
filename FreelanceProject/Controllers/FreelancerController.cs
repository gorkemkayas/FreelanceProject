using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.Controllers
{
    public class FreelancerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MyApplications()
        {
            return View();
        }
    }
}

using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.Controllers
{
    public class ApplicationsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create() // işi almak isteyen kullanıcının başvuru yaptığı sayfa
        {
            return View();
        }
       
    }
}

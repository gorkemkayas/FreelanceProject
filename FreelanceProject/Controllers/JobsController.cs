using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.Controllers
{
    public class JobsController : Controller
    {
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
        public IActionResult Edit()  // İş ilanını düzenleme
        {
             return View();
        }
        public IActionResult Delete()  // İş ilanını  silme
        {
            return View();
        }
    }
}

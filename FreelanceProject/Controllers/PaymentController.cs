using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.Controllers
{
    public class PaymentController : Controller
    {
        public IActionResult Summary()
        {
            return View();
        }
    }
}

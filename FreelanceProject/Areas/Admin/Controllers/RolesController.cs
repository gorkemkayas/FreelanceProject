using FreelanceProject.Areas.Admin.Models;
using FreelanceProject.Extensions;
using FreelanceProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(RoleAddViewModel request)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please fill in all required fields.");
                return View(request);
            }

            var result = await _roleService.AddRoleAsync(request);

            if(!result.IsSuccess)
            {
                TempData["Failed"] = "A problem occured while adding role.";
                ModelState.AddModelErrorList(result.Errors);
                return View(request);
            }
            TempData["Success"] = "Role added successfully.";
            return RedirectToAction(nameof(DashboardController.Index), "Dashboard");
        }

        [HttpGet]
        public async Task<IActionResult> RoleList(int page = 1, int pageSize = 4, bool includeDeleted = false)
        {
            var pagedRoles = await _roleService.GetPagedRolesAsync(page, pageSize, includeDeleted);
            pagedRoles.IncludeDeleted = includeDeleted;
            return View(pagedRoles);
        }
    }
}

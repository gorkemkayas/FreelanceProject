using FreelanceProject.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserList(int page = 1, int pageSize = 50, bool includeDeleted = false)
        {
            var pagedUsers = await _userService.GetPagedUsersAsync(page, pageSize, includeDeleted);
            pagedUsers.IncludeDeleted = includeDeleted;
            return View(pagedUsers);
        }
    }
}

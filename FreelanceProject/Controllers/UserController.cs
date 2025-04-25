using FreelanceProject.Data.Entities;
using FreelanceProject.Extensions;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> SignIn(SignInViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var result = await _userService.SignInAsync(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }
            TempData["Succeed"] = "The user signed in successfully!";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var result = await _userService.CreateUserAsync(request);

            if(!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }

            TempData["Succeed"] = "The user created successfully!";

            return RedirectToAction(nameof(HomeController.SignIn),"User");
        }

        public async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();
            TempData["Succeed"] = "The user signed out successfully!";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult Profile()
        {
            return View();
        }
    }
}

using FreelanceProject.CustomMethods.Abstract;
using FreelanceProject.Data.Entities;
using FreelanceProject.Extensions;
using FreelanceProject.Models.ViewModels;
using FreelanceProject.Services.Abstract;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using Microsoft.Identity.Client;

namespace FreelanceProject.Controllers
{
    public partial class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly IUrlGenerator _urlGenerator;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public UserController(IUserService userService, IEmailService emailService, IUrlGenerator urlGenerator, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userService = userService;
            _emailService = emailService;
            _urlGenerator = urlGenerator;
            _userManager = userManager;
            _signInManager = signInManager;
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
                if(result.Errors.Any(x => x.Code == "EmailNotConfirmed"))
                {
                    TempData["Error"] = "Please confirm your email address!";
                    TempData["EmailNotConfirmed"] = true;
                    TempData["UserEmail"] = request.Email;
                }
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

            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View();
            }

            TempData["Succeed"] = "The user created successfully!";

            var token = await _userService.GenerateEmailConfirmationTokenAsync(result.Data!);
            var confirmationLink = _urlGenerator.GenerateEmailConfirmationUrl(result.Data!, token);

            await _emailService.SendEmailConfirmationEmailAsync(result.Data!.Email!, confirmationLink);

            return RedirectToAction("ConfirmEmail","User");
        }

        public async Task<IActionResult> SignOut()
        {
            await _userService.SignOutAsync();
            TempData["Succeed"] = "The user signed out successfully!";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResendEmailConfirmation([FromBody] ResendEmailRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return BadRequest();
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var confirmationLink = _urlGenerator.GenerateEmailConfirmationUrl(user, token);

            await _emailService.SendEmailConfirmationEmailAsync(user.Email!, confirmationLink);

            return Ok();
        }

        public async Task<IActionResult> Profile(string? userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var visitedUser = await _userService.FindByNameAsync(userName);

            if (visitedUser == null)
            {
                TempData["Error"] = "User not found!";
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == visitedUser)
            {
                ViewBag.IsOwner = true;

                var extendedUser = _userService.GetExtendedProfileViewModel(currentUser);
                return View(extendedUser);
            }

            var visitorUser = _userService.GetVisitorProfileViewModel(visitedUser);
            return View(visitorUser);
        }


        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var user = await _userService.FindByEmailAsync(request.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "User not found!");
                return View(request);
            }

            var resetToken = await _userService.GeneratePasswordResetTokenAsync(user);

            var resetPasswordLink = _urlGenerator.GenerateResetPasswordUrl(user, resetToken);

            await _emailService.SendForgetPasswordEmailAsync(user.Email!, resetPasswordLink);

            TempData["Succeed"] = "Reset password link has been sent to your email!";
            return View(request);
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            TempData["UserId"] = userId;
            TempData["Token"] = token;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "Please fill the areas correctly.");
                return View(request);
            }
            var token = TempData["Token"]?.ToString();
            var userId = TempData["UserId"]?.ToString();

            if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError(string.Empty, "Token or UserId is null");
                return View(request);
            }

            var result = await _userService.ResetPasswordAsync(request.NewPassword, userId, token);

            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View(request);
            }

            TempData["Succeed"] = "Password reset successfully!";
            var user = await _userManager.FindByIdAsync(userId);

            await _userManager.UpdateSecurityStampAsync(user!);

            return RedirectToAction(nameof(HomeController.SignIn), "User");


        }

        [HttpGet]
        public IActionResult ConfirmEmail(string? userId, string? token)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                return View();
            }
            ViewBag.UserId = userId;
            ViewBag.Token = token;
            return View(new ConfirmEmailViewModel() { UserId = userId, Token = token});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel request)
        {
            var result = await _userService.ConfirmEmailAsync(request);
            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors);
                return View(request);
            }
            TempData["Succeed"] = "Email confirmed successfully!";

            return RedirectToAction(nameof(UserController.SignIn));
            
            //confirm edildiyse confirmEmail sayfasında 'basarıyla dogrulandı' mesajı ver
            //confirm edilmediyse 'dogrulama hatası' mesajı ver
            // yönlendirme olmadan girildiyse hata sayfasına yönlendir
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(ExtendedProfileViewModel request, IFormFile? fileInputProfile, IFormFile? coverInputProfile, IFormFile? IconInputWorkingAt)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _userService.UpdateProfileAsync((await _userManager.GetUserAsync(User!))!, request, fileInputProfile, coverInputProfile, IconInputWorkingAt);

            if (!result.IsSuccess)
            {
                ModelState.AddModelErrorList(result.Errors!.ToList());

                TempData["Failed"] = "An error occurred while updating the profile.";
                return RedirectToAction(nameof(Profile), new { userName = User.Identity!.Name });
            }

            if (result.isCritical)
            {
                await _userService.SignOutAsync();
                await _signInManager.SignInAsync((await _userManager.FindByIdAsync(request.Id))!,true);

                TempData["Succeed"] = "Profile updated successfully.";

                return RedirectToAction(nameof(Profile), new { userName = User.Identity!.Name });
            }

            TempData["Succeed"] = "Profile updated successfully.";

            return RedirectToAction(nameof(Profile), new { userName = User.Identity!.Name });
        }
    }
}

using FreelanceProject.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.ViewComponents
{
    public class NavProfilePhotoViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public NavProfilePhotoViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = (await _userManager.GetUserAsync(HttpContext.User))!;
            var profilePhoto = $"/img/userPhotos/{user.UserName}//{user.ProfilePicture}";
            if (user.ProfilePicture == null)
            {
                return View("Default", "/img/defaultProfilePhoto.jpg");
            }

            return View("Default", profilePhoto);
        }
    }
}

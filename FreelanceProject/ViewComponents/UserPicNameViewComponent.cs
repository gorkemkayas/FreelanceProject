using FreelanceProject.Data.Entities;
using FreelanceProject.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.ViewComponents
{
    public class UserPicNameViewComponent :ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;

        public UserPicNameViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            if (userId == null)
            {
                return View("Default");
            }
            var user = await _userManager.FindByIdAsync(userId);


            var userPhotoLink = $"/img/userPhotos/{user!.UserName}//{user.ProfilePicture}";
            if (user.ProfilePicture == null)
            {
                userPhotoLink = "/img/defaultProfilePhoto.jpg";
            }

            var info = new UserProfileAndUserName() { UserName = user.UserName!, UserPhotoLink = userPhotoLink };

            return View("Default", info);
        }
    }
}

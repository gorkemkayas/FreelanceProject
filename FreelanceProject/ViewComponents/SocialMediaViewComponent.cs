using FreelanceProject.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FreelanceProject.ViewComponents
{
    public class SocialMediaViewComponent : ViewComponent
    {
        private readonly UserManager<AppUser> _userManager;
        public SocialMediaViewComponent(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync(string UserName)
        {
            var user = (await _userManager.FindByNameAsync(UserName))!;

            var socialmediaInformations = new
            {
                Linkedin = user.LinkedinAddress,
                Github = user.GithubAddress,
                Medium = user.MediumAddress,
                X = user.XAddress,
                Youtube = user.YoutubeAddress,
                PersonalWeb = user.PersonalWebAddress
            };

            return View("Default", socialmediaInformations);
        }
    }
}

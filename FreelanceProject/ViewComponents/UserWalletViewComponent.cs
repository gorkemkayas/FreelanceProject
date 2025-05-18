using FreelanceProject.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceProject.ViewComponents
{

    public class UserWalletViewComponent : ViewComponent
    {
        private readonly FreelanceDbContext _context;

        public UserWalletViewComponent(FreelanceDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user == null)
            {
                return View("Default");
            }

            var wallet = user.Wallet;
            return View("Default", wallet);
        }
    }
}

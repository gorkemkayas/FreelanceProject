using FreelanceProject.Data.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreelanceProject.ViewComponents
{
    public class SystemWalletViewComponent :ViewComponent
    {
        private readonly FreelanceDbContext _context;

        public SystemWalletViewComponent(FreelanceDbContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id.ToString() == "c583f39c-6e40-4e34-b852-08dd9633dfd1");
            if (user == null)
            {
                return View("Default");
            }

            var wallet = user.Wallet;
            return View("Default", wallet);
        }
    }
}

using FreelanceProject.Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FreelanceProject.Data
{
    public class FreelanceDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public FreelanceDbContext(DbContextOptions options) : base(options)
        {

        }

    }
}

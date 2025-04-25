using Microsoft.AspNetCore.Identity;

namespace FreelanceProject.Data.Entities
{
    public class AppUser: IdentityUser<Guid>
    {
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public DateTime BirthDate { get; set; }
    }
}

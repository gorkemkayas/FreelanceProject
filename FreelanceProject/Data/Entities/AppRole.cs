using Microsoft.AspNetCore.Identity;

namespace FreelanceProject.Data.Entities
{
    public class AppRole : IdentityRole<Guid>
    {
        public string? CreatedBy { get; set; }
        public string? EditedBy { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}

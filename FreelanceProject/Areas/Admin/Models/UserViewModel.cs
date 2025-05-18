namespace FreelanceProject.Areas.Admin.Models
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }

        public bool IsDeleted { get; set; } = false; // Indicates if the role is deleted or not.
        
        public string Email { get; set; } = null!;
        public string? PhoneNumber { get; set; }
    }
}

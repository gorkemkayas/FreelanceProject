using System.ComponentModel.DataAnnotations;

namespace FreelanceProject.Areas.Admin.Models
{
        public class RoleAddViewModel
        {
            [Required(ErrorMessage = "Role Name field cannot be blank.")]
            public string Name { get; set; }
            public string CreatedBy { get; set; }
        }
}

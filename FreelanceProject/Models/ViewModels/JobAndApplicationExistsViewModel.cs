using FreelanceProject.Data.Entities;

namespace FreelanceProject.Models.ViewModels
{
    public class JobAndApplicationExistsViewModel
    {
        public JobEntity Job { get; set; } = null!;
        public bool Applied { get; set; }
    }
}

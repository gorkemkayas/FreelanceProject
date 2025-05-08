using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class CategoryEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDraft { get; set; }
        public virtual ICollection<PostEntity>? Jobs { get; set; }
    }
}

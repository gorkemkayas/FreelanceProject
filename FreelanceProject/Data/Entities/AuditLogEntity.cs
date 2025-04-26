using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class AuditLogEntity : BaseEntity
    {
        public Guid? UserId { get; set; }
        public string Operation { get; set; } = null!;
        public string TableName { get; set; } = null!;
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
    }
}

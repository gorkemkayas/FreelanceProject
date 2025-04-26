using FreelanceProject.Data.Entities.BaseEntities;
using FreelanceProject.Utilities;

namespace FreelanceProject.Data.Entities
{
    public class NotificationEntity : BaseEntity
    {
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public NotificationType NotificationType { get; set; }

        public Guid UserId { get; set; }
        public AppUser User { get; set; }
    }
}

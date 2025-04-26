using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class MessageEntity : BaseEntity
    {
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }

        public Guid? SenderId { get; set; }
        public virtual AppUser? Sender { get; set; }

        public Guid? ReceiverId { get; set; }
        public virtual AppUser? Receiver { get; set; }
    }
}

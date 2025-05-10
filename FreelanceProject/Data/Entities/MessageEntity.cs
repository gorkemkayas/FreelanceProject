using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class MessageEntity : BaseEntity
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime SentDate { get; set; }

        public Guid? SenderId { get; set; }
        public virtual AppUser? Sender { get; set; }

        public Guid? ReceiverId { get; set; }
        public virtual AppUser? Receiver { get; set; }

        public Guid? JobId { get; set; } // 💬 Hangi işe ait mesaj
        public virtual JobEntity? Job { get; set; }


    }
}

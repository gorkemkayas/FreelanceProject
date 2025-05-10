using FreelanceProject.Data.Entities;

namespace FreelanceProject.Models.ViewModels
{
    public class ChatViewModel
    {
        public JobEntity Job { get; set; } // Mesajın hangi işe ait olduğunu tutuyoruz
        public List<MessageEntity> Messages { get; set; } = new List<MessageEntity>(); // İlgili iş için mesajlar
        public string NewMessageContent { get; set; } // Yeni gönderilen mesajın içeriği
        public Guid ReceiverId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageDate { get; set; }
        public Guid JobId { get; set; }

        public List<MessageThreadViewModel> Threads { get; set; } = new List<MessageThreadViewModel>();

    }
}

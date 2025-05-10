namespace FreelanceProject.Models.ViewModels
{
    public class MessageThreadViewModel
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string LastMessage { get; set; }
        public DateTime LastMessageDate { get; set; }
        public Guid JobId { get; set; }
        public Guid ReceiverId { get; set; }
        public Guid OwnerId { get; set; }

    }
}

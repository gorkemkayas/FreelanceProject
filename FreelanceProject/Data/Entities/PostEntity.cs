using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class PostEntity : BaseEntity
    {
        public string Title { get; set; }
        public string? Subtitle { get; set; }
        public string Content { get; set; }
        public string? SubContent { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool IsDraft { get; set; }
        public int ViewCount { get; set; }

        public Guid AuthorId { get; set; }
        public AppUser Author { get; set; }

        public Guid CategoryId { get; set; }
        public virtual CategoryEntity Category { get; set; }

        //public Guid CommentId { get; set; }
        public virtual ICollection<CommentEntity>? Comments { get; set; }

        public virtual ICollection<LikeEntity>? Likes { get; set; }
    }
}

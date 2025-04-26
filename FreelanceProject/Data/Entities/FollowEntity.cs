using FreelanceProject.Data.Entities.BaseEntities;

namespace FreelanceProject.Data.Entities
{
    public class FollowEntity : BaseEntity
    {
        public Guid FollowerId { get; set; }
        public AppUser Follower { get; set; }

        public Guid FollowingId { get; set; }
        public AppUser Following { get; set; }
    }
}

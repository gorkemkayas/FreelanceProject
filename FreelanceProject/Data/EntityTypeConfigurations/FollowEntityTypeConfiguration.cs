using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class FollowEntityTypeConfiguration : BaseEntityTypeConfiguration<FollowEntity>
    {
        public override void Configure(EntityTypeBuilder<FollowEntity> builder)
        {
            builder.HasIndex(f => new { f.FollowerId, f.FollowingId })
                   .IsUnique();

            builder.HasOne(e => e.Follower)
                .WithMany(u => u.Followings)
                .HasForeignKey(f => f.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Following)
                .WithMany(u => u.Followers)
                .HasForeignKey(f => f.FollowingId)
                .OnDelete(DeleteBehavior.Restrict);


            base.Configure(builder);
        }
    }
    
}

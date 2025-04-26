using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class LikeEntityTypeConfiguration : BaseEntityTypeConfiguration<LikeEntity>
    {
        public override void Configure(EntityTypeBuilder<LikeEntity> builder)
        {
            builder.HasOne(e => e.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(f => f.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            base.Configure(builder);
        }
    }
    
}

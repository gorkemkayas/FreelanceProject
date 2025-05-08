using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class PostEntityTypeConfiguration : BaseEntityTypeConfiguration<PostEntity>
    {
        public override void Configure(EntityTypeBuilder<PostEntity> builder)
        {
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            
            builder.Property(e => e.Subtitle)
                   .IsRequired()
                   .HasMaxLength(100);

            
            builder.Property(e => e.Content)
                   .IsRequired()
                   .HasMaxLength(5000);

            builder.Property(e => e.IsDraft).HasDefaultValue(false);

            builder.Property(e => e.ViewCount).HasDefaultValue(0);

            builder.HasOne(e => e.Author)
                .WithMany(a => a.Posts)
                .HasForeignKey(e => e.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.Category)
                .WithMany(c => c.Jobs)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Comments)
                .WithOne(c => c.Post)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.Likes)
                .WithOne(l => l.Post)
                .HasForeignKey(l => l.PostId)
                .OnDelete(DeleteBehavior.Restrict);


            base.Configure(builder);
        }
    }
}

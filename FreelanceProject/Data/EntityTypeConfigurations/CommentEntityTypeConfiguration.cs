using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class CommentEntityTypeConfiguration : BaseEntityTypeConfiguration<CommentEntity>
    {
        public override void Configure(EntityTypeBuilder<CommentEntity> builder)
        {
            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.HasOne(e => e.Post)
                .WithMany(e => e.Comments)
                .HasForeignKey(e => e.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(c => c.Author)
                   .WithMany(a => a.Comments)
                   .HasForeignKey(f => f.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            
            builder.HasOne(c => c.ParentComment)
                   .WithMany(pc => pc.Replies)
                   .HasForeignKey(f => f.ParentCommentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Likes)
                .WithOne(c => c.Comment)
                .HasForeignKey(c => c.CommentId)
                .OnDelete(DeleteBehavior.Restrict);


            base.Configure(builder);
        }
    }
}

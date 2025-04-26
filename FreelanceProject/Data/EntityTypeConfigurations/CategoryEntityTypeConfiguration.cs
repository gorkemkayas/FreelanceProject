using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class CategoryEntityTypeConfiguration : BaseEntityTypeConfiguration<CategoryEntity>
    {
        public override void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(e => e.IsDraft)
                .IsRequired()
                .HasDefaultValue(false);

            builder.HasMany(e => e.Posts)
                .WithOne(e => e.Category)
                .HasForeignKey(e => e.CategoryId);

            base.Configure(builder);
        }
    }
}

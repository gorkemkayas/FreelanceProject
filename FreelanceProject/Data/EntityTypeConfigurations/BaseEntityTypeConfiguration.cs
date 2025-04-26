using FreelanceProject.Data.Entities.BaseEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.CreatedDate)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(e => e.DeletedDate)
                .HasColumnType("datetime2")
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .IsRequired(true);
        }
    }
}

using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class JobEntityTypeConfiguration : BaseEntityTypeConfiguration<JobEntity>
    {
        public override void Configure(EntityTypeBuilder<JobEntity> builder)
        {

            builder.HasKey(j => j.Id); // Primary key için Id'yi kullanıyoruz

            builder.Property(j => j.Title)
                .IsRequired() // Başlık zorunlu
                .HasMaxLength(100); // Maksimum 100 karakter

            builder.Property(j => j.Description)
                .IsRequired() // Açıklama zorunlu
                .HasMaxLength(500); // Maksimum 500 karakter

            builder.Property(j => j.Requirements)
                   .HasMaxLength(1000); // Optional alana da limit getirildi

            builder.Property(j => j.JobImage)
                .HasMaxLength(500); // URL ya da dosya yolu için yeterli

            builder.Property(j => j.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()"); // İlan oluşturulma tarihi

            builder.Property(j => j.IsDeleted)
                .HasDefaultValue(false); // Soft delete için varsayılan değer

            // Foreign Key ve ilişkiler
            builder.HasOne(j => j.Owner) // Her işin bir sahibi (AppUser)
                .WithMany(u => u.CreatedJobs) // Kullanıcılar birden fazla iş oluşturabilir
                .HasForeignKey(j => j.OwnerId) // OwnerId foreign key olarak
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silindiğinde ilan silinmesin

            builder.HasMany(j => j.JobApplications) // Bir iş birden fazla başvuru alabilir
                .WithOne(a => a.Job) // Her başvuru bir işe ait
                .HasForeignKey(a => a.JobId) // JobId foreign key olarak
                .OnDelete(DeleteBehavior.Restrict); // Başvuru silindiğinde iş silinmesin

            base.Configure(builder);
        }
    }
}

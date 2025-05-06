using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class JobApplicationEntityTypeConfiguration : BaseEntityTypeConfiguration<JobApplicationEntity>
    {
        public override void Configure(EntityTypeBuilder<JobApplicationEntity> builder)
        {
            builder.HasKey(ja => ja.Id); // Birincil anahtar

            builder.Property(ja => ja.AppliedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()"); // Varsayılan başvuru tarihi

            builder.Property(ja => ja.CoverLetter)
                .HasMaxLength(1000); // Kapak mektubu uzunluğu

            // Enum tipi property için konfigürasyon
            builder.Property(ja => ja.Status)
                .HasConversion(
                    v => v.ToString(),  // Enum'u string'e dönüştürme
                    v => (JobApplicationStatus)Enum.Parse(typeof(JobApplicationStatus), v)) // String'i enum'a dönüştürme
                .HasDefaultValue(JobApplicationStatus.Pending); // Varsayılan değer

            builder.HasOne(ja => ja.Job)
                .WithMany(j => j.Applications)
                .HasForeignKey(ja => ja.JobId)
                .OnDelete(DeleteBehavior.Restrict); // İlan silinirse başvuruları silme (restrict)

            builder.HasOne(ja => ja.Applicant)
                .WithMany(u => u.JobApplications)
                .HasForeignKey(ja => ja.ApplicantId)
                .OnDelete(DeleteBehavior.Restrict); // Kullanıcı silinirse başvurular silinmesin (restrict)

            builder.Property(ja => ja.IsDeleted).HasDefaultValue(false); // Soft delete özelliği

            base.Configure(builder);
        }
    }
}

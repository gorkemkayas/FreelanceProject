using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class AppUserEntityTypeConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(p => p.RegisteredDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETDATE()");

            builder.HasMany(u => u.CreatedJobs)
                   .WithOne(j => j.Owner)
                   .HasForeignKey(f => f.OwnerId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.JobApplications)
                   .WithOne(j => j.Applicant)
                   .HasForeignKey(f => f.ApplicantId)
                   .OnDelete(DeleteBehavior.Restrict);

            //  basit yapı ayarları

            builder.Property(u => u.Name)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(u => u.Surname)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.Title)
                .HasMaxLength(100);

            builder.Property(u => u.Bio)
                .HasMaxLength(1000);

            builder.Property(u => u.WorkingAt)
                .HasMaxLength(100);

            builder.Property(u => u.WorkingAtLogo)
                .HasMaxLength(500);

            builder.Property(u => u.CurrentPosition)
                .HasMaxLength(100);

            builder.Property(u => u.City)
                .HasMaxLength(100);

            builder.Property(u => u.Address)
                .HasMaxLength(300);

            builder.Property(u => u.XAddress)
                .HasMaxLength(300);

            builder.Property(u => u.LinkedinAddress)
                .HasMaxLength(300);

            builder.Property(u => u.GithubAddress)
                .HasMaxLength(300);

            builder.Property(u => u.MediumAddress)
                .HasMaxLength(300);

            builder.Property(u => u.YoutubeAddress)
                .HasMaxLength(300);

            builder.Property(u => u.PersonalWebAddress)
                .HasMaxLength(300);

            builder.Property(u => u.HighSchoolName)
                .HasMaxLength(150);

            builder.Property(u => u.HighSchoolStartYear)
                .HasMaxLength(4); // "2020" gibi 4 karakterlik yıl formatı için

            builder.Property(u => u.HighSchoolGraduationYear)
                .HasMaxLength(4);

            builder.Property(u => u.UniversityName)
                .HasMaxLength(150);

            builder.Property(u => u.UniversityStartYear)
                .HasMaxLength(4);

            builder.Property(u => u.UniversityGraduationYear)
                .HasMaxLength(4);

            builder.Property(u => u.ProfilePicture)
                .HasMaxLength(500);

            builder.Property(u => u.CoverImagePicture)
                .HasMaxLength(500);
        }
    }
}

using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class NotificationEntityTypeConfiguration : BaseEntityTypeConfiguration<NotificationEntity>
    {
        public override void Configure(EntityTypeBuilder<NotificationEntity> builder)
        {
            builder.Property(e => e.Message)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.Property(e => e.IsRead)
                   .HasDefaultValue(false);

            builder.Property(e => e.NotificationType)
                   .IsRequired();

            builder.HasOne(e => e.User)
                   .WithMany(u => u.Notifications)
                   .HasForeignKey(f => f.UserId);

            base.Configure(builder);
        }
    }
}

using FreelanceProject.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FreelanceProject.Data.EntityTypeConfigurations
{
    public class MessageEntityTypeConfiguration : BaseEntityTypeConfiguration<MessageEntity>
    {
        public override void Configure(EntityTypeBuilder<MessageEntity> builder)
        {
            builder.Property(e => e.Content)
                .IsRequired()
                .HasMaxLength(1000);

            builder.Property(e => e.IsRead)
                .HasDefaultValue(false);

            builder.Property(e => e.SentDate)
                .HasColumnType("datetime2")
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()");

            builder.HasOne(e => e.Sender)
                .WithMany(u => u.SentMessages)
                .HasForeignKey(f => f.SenderId);

            builder.HasOne(e => e.Receiver)
                .WithMany(u => u.ReceivedMessages)
                .HasForeignKey(f => f.ReceiverId);

            base.Configure(builder);
        }
    }
}

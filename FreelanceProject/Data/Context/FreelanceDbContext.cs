﻿using FreelanceProject.Data.Entities;
using FreelanceProject.Data.Entities.BaseEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FreelanceProject.Data.Context
{
    public class FreelanceDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public FreelanceDbContext(DbContextOptions options) : base(options)
        {
        }


        public DbSet<CategoryEntity> Categories { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<FollowEntity> Follows { get; set; }
        public DbSet<LikeEntity> Likes { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<NotificationEntity> Notifications { get; set; }
        public DbSet<PostEntity> Posts { get; set; }

        public DbSet<AuditLogEntity> AuditLogs { get; set; }
        public DbSet<JobEntity> Jobs { get; set; }
        public DbSet<JobApplicationEntity> JobApplications { get; set; }

        public DbSet<JobApplicationEntity> JobApplicationEntity { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            //builder.ApplyConfigurationsFromAssembly(typeof(FreelanceDbContext).Assembly, predicate => predicate.Namespace == "FreelanceProject.Data.EntityTypeConfigurations");

            //base.OnModelCreating(builder);
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(FreelanceDbContext).Assembly, predicate => predicate.Namespace == "FreelanceProject.Data.EntityTypeConfigurations");

            builder.Entity<JobEntity>(entity =>
            {
                entity.Property(e => e.Budget)
                      .HasPrecision(18, 2); // 18 basamaklı, 2 ondalıklı
            });

            builder.Entity<MessageEntity>()
.HasOne(m => m.Job)
.WithMany() // Eğer Job → Message ilişkisi tanımlı değilse boş bırak
.HasForeignKey(m => m.JobId)
.OnDelete(DeleteBehavior.Restrict); // Silindiğinde mesajlar silinmesin

            // Diğer ilişkiler (opsiyonel)
            builder.Entity<MessageEntity>()
                .HasOne(m => m.Sender)
                .WithMany()
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<MessageEntity>()
                .HasOne(m => m.Receiver)
                .WithMany()
                .HasForeignKey(m => m.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            // JobApplicationEntity için benzersiz kısıtlama ekliyoruz
            builder.Entity<JobApplicationEntity>()
                .HasIndex(ja => new { ja.ApplicantId, ja.JobId })
                .IsUnique(); // Aynı kullanıcı aynı işe birden fazla başvuru yapamaz


        }


    }
}

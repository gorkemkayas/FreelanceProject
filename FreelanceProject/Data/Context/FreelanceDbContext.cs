using FreelanceProject.Data.Entities;
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

        }


    }
}
